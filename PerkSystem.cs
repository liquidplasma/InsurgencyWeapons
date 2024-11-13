using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace InsurgencyWeapons
{
    public class ProjPerkTracking : GlobalProjectile
    {
        public bool
            ShotFromInsurgencyWeapon,
            Grenade;

        public int Perk;

        public override bool InstancePerEntity => true;
    }

    public class PerkSystem : ModPlayer
    {
        public enum Perks
        {
            SupportSpecialist,

            Commando,

            Demolitons,

            Sharpshooter
        }

        private enum Bosses
        {
            NoBoss,

            Skeletron,

            WallOfFlesh,

            AllMechs,

            Plantera,

            Cultist,

            Moonlord
        }

        private int BossDowned()
        {
            int x = 0;
            if (NPC.downedBoss3)
                x++;
            if (Main.hardMode)
                x++;
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                x++;
            if (NPC.downedPlantBoss)
                x++;
            if (NPC.downedAncientCultist)
                x++;
            if (NPC.downedMoonlord)
                x++;
            return x;
        }

        public Dictionary<int, string> ClassNames = new()
        {
            {(int)Perks.Commando, Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.CommandoClass") },
            {(int)Perks.Demolitons, Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.DemolitionsClass") },
            {(int)Perks.SupportSpecialist, Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.SupportSpecialistClass") },
            {(int)Perks.Sharpshooter, Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.SharpshooterClass") }
        };

        public bool CommandoWeapons(Item item) =>
            Insurgency.AssaultRifles.Contains(item.type) ||
            Insurgency.SubMachineGuns.Contains(item.type) ||
            Insurgency.Carbines.Contains(item.type) ||
            Insurgency.BattleRifles.Contains(item.type) ||
            Insurgency.LightMachineGuns.Contains(item.type);

        public bool SharpshooterWeapons(Item item) =>
            Insurgency.SniperRifles.Contains(item.type) ||
            Insurgency.Rifles.Contains(item.type) ||
            Insurgency.Pistols.Contains(item.type) ||
            Insurgency.Revolvers.Contains(item.type);

        public bool SupportSpecialistWeapons(Item item) => Insurgency.Shotguns.Contains(item.type);

        public bool DemolitionsWeapons(Item item) => Insurgency.Grenades.Contains(item.type) || Insurgency.Launchers.Contains(item.type);

        /// <summary>
        /// Assault Rifles, SMGs, Carbines, Battle Rifles and LMGs
        /// </summary>
        public int CommandoDamage, CommandoKills;

        /// <summary>
        /// Shotguns
        /// </summary>
        public int SupportSpecialistDamage, SupportSpecialistKills;

        /// <summary>
        /// Explosives
        /// </summary>
        public int DemolitionsDamage, DemolitionsKills;

        /// <summary>
        /// Snipers, rifles, pistols and revolvers
        /// </summary>
        public int SharpShooterDamage, SharpShooterKills;

        public int[] Level = new int[4];

        public int[] DamageRequired = [1000, 4000, 16000, 64000, 384000, 3072000];

        public int[] KillsRequired = [10, 30, 90, 270, 540, 1080];

        public float GetDamageMultPerLevel(int perk)
        {
            if (Level[perk] == 0)
                return 2 / 60f;

            switch (BossDowned())
            {
                case (int)Bosses.NoBoss:
                    return Level[perk] * (4 / 60f);

                case (int)Bosses.Skeletron:
                    return Level[perk] * (6 / 60f);

                case (int)Bosses.WallOfFlesh:
                    return Level[perk] * (8 / 60f);

                case (int)Bosses.AllMechs:
                    return Level[perk] * (10 / 60f);

                case (int)Bosses.Plantera:
                    return Level[perk] * (14 / 60f);

                case (int)Bosses.Cultist:
                    return Level[perk] * (16 / 60f);

                case (int)Bosses.Moonlord:
                    return Level[perk] * (30 / 60f);

                default:
                    break;
            }
            return 0;
        }

        public float GetPenetrationBuffSupport()
        {
            if (Level[(int)Perks.SupportSpecialist] == 0)
                return 1f;
            return 1f + Level[(int)Perks.SupportSpecialist] * (9 / 60f);
        }

        public void UpdateLevels(int currentDamage, int currentKills, int perk)
        {
            if (currentDamage > DamageRequired[Level[perk]])
            {
                switch (perk)
                {
                    case (int)Perks.Commando:
                        CommandoDamage = DamageRequired[Level[perk]];
                        break;

                    case (int)Perks.SupportSpecialist:
                        SupportSpecialistDamage = DamageRequired[Level[perk]];
                        break;

                    case (int)Perks.Demolitons:
                        DemolitionsDamage = DamageRequired[Level[perk]];
                        break;

                    case (int)Perks.Sharpshooter:
                        SharpShooterDamage = DamageRequired[Level[perk]];
                        break;
                }
            }
            if (currentKills > KillsRequired[Level[perk]])
            {
                switch (perk)
                {
                    case (int)Perks.Commando:
                        CommandoKills = KillsRequired[Level[perk]];
                        break;

                    case (int)Perks.SupportSpecialist:
                        SupportSpecialistKills = KillsRequired[Level[perk]];
                        break;

                    case (int)Perks.Demolitons:
                        DemolitionsKills = KillsRequired[Level[perk]];
                        break;

                    case (int)Perks.Sharpshooter:
                        SharpShooterKills = KillsRequired[Level[perk]];
                        break;
                }
            }
            if (currentDamage >= DamageRequired[Level[perk]] && currentKills >= KillsRequired[Level[perk]])
            {
                if (Level[perk] >= 6)
                    return;
                Level[perk] += 1;

                CombatText LevelUp = CreateCombatText(Player, Color.Green, ClassNames[perk] + " " + Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.LevelUp"));
                LevelUp.lifeTime = 240;
                if (LevelUp.lifeTime == 120)
                    LevelUp.color = Color.Red;

                switch (perk)
                {
                    case (int)Perks.Commando:
                        CommandoKills = CommandoDamage = 0;
                        break;

                    case (int)Perks.SupportSpecialist:
                        SupportSpecialistKills = SupportSpecialistDamage = 0;
                        break;

                    case (int)Perks.Demolitons:
                        DemolitionsKills = DemolitionsDamage = 0;
                        break;

                    case (int)Perks.Sharpshooter:
                        SharpShooterKills = SharpShooterDamage = 0;
                        break;
                }
            }
        }

        private static ProjPerkTracking GlobalPerk(Projectile proj)
        {
            return proj.GetGlobalProjectile<ProjPerkTracking>();
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (GlobalPerk(proj).ShotFromInsurgencyWeapon && proj.owner == Player.whoAmI)
            {
                if (target.type == NPCID.TargetDummy)
                    return;

                switch (proj.ai[0])
                {
                    case (int)Perks.Commando:
                        CommandoDamage += hit.Damage;
                        if (target.life <= 0)
                            CommandoKills++;
                        UpdateLevels(CommandoDamage, CommandoKills, (int)Perks.Commando);
                        break;

                    case (int)Perks.SupportSpecialist:
                        SupportSpecialistDamage += hit.Damage;
                        if (target.life <= 0)
                            SupportSpecialistKills++;
                        UpdateLevels(SupportSpecialistDamage, SupportSpecialistKills, (int)Perks.SupportSpecialist);
                        break;

                    case (int)Perks.Demolitons:
                        DemolitionsDamage += hit.Damage;
                        if (target.life <= 0)
                            DemolitionsKills++;
                        UpdateLevels(DemolitionsDamage, DemolitionsKills, (int)Perks.Demolitons);
                        break;

                    case (int)Perks.Sharpshooter:
                        SharpShooterDamage += hit.Damage;
                        if (target.life <= 0)
                            SharpShooterKills++;
                        UpdateLevels(SharpShooterDamage, SharpShooterKills, (int)Perks.Sharpshooter);
                        break;
                }
            }

            if (GlobalPerk(proj).Grenade && proj.owner == Player.whoAmI)
            {
                if (target.type == NPCID.TargetDummy)
                    return;

                DemolitionsDamage += hit.Damage;
                if (target.life <= 0)
                    DemolitionsKills++;
                UpdateLevels(DemolitionsDamage, DemolitionsKills, (int)Perks.Demolitons);
            }
        }

        public override void SaveData(TagCompound tag)
        {
            if (CommandoDamage > 0)
                tag["INSCommandoDamage"] = CommandoDamage;
            if (CommandoKills > 0)
                tag["INSCommandoKills"] = CommandoKills;
            if (SupportSpecialistDamage > 0)
                tag["INSSupportDamage"] = SupportSpecialistDamage;
            if (SupportSpecialistKills > 0)
                tag["INSSupportKills"] = SupportSpecialistKills;
            if (DemolitionsDamage > 0)
                tag["INSDemolitionsDamage"] = DemolitionsDamage;
            if (DemolitionsKills > 0)
                tag["INSDemolitionsKills"] = DemolitionsKills;
            if (SharpShooterDamage > 0)
                tag["INSSharpshooterDamage"] = SharpShooterDamage;
            if (SharpShooterKills > 0)
                tag["INSSharpshooterKills"] = SharpShooterKills;
            tag["INSLevel"] = Level;
        }

        public override void LoadData(TagCompound tag)
        {
            CommandoDamage = tag.GetInt("INSCommandoDamage");
            CommandoKills = tag.GetInt("INSCommandoKills");
            SupportSpecialistDamage = tag.GetInt("INSSupportDamage");
            SupportSpecialistKills = tag.GetInt("INSSupportKills");
            DemolitionsDamage = tag.GetInt("INSDemolitionsDamage");
            DemolitionsKills = tag.GetInt("INSDemolitionsKills");
            SharpShooterDamage = tag.GetInt("INSSharpshooterDamage");
            SharpShooterKills = tag.GetInt("INSSharpshooterKills");
            Level = tag.GetIntArray("INSLevel");
        }
    }

    public class ToolTipShowPerkProgress : GlobalItem
    {
        private static LocalizedText PenBuff { get; set; }
        private Player Player => Main.LocalPlayer;
        private PerkSystem PerkTracking => Player.GetModPlayer<PerkSystem>();

        public override void SetStaticDefaults()
        {
            PenBuff = Language.GetText("Mods.InsurgencyWeapons.PerkSystem.PenPowerAmount");
            base.SetStaticDefaults();
        }

        private void BuildTooltip(List<TooltipLine> tooltips, int perk, int damage, int kills, Item item = null)
        {
            TooltipLine Class = new(Mod, "ClassDisplay", PerkTracking.ClassNames[perk])
            {
                OverrideColor = Color.DodgerBlue
            };
            tooltips.Add(Class);

            TooltipLine Level = new(Mod, "Level", Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.Level") + " " + PerkTracking.Level[perk])
            {
                OverrideColor = Color.PaleVioletRed,
            };
            tooltips.Add(Level);

            int maxDamage = PerkTracking.DamageRequired[PerkTracking.Level[perk]];
            TooltipLine DamageRequired = new(Mod, "DamageRequired", Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.DamageRequired") + " " + damage + "/" + maxDamage)
            {
                OverrideColor = Color.YellowGreen,
            };
            tooltips.Add(DamageRequired);

            int maxKills = PerkTracking.KillsRequired[PerkTracking.Level[perk]];
            TooltipLine KillsRequired = new(Mod, "KillsRequired", Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.KillsRequired") + " " + kills + "/" + maxKills)
            {
                OverrideColor = Color.YellowGreen,
            };
            tooltips.Add(KillsRequired);

            int buff = (int)(PerkTracking.GetDamageMultPerLevel(perk) * 100);
            TooltipLine CurrentBuff = new(Mod, "CurrentBuff", buff + "% " + Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.CurrentBuff"))
            {
                OverrideColor = Color.LightGreen,
            };
            tooltips.Add(CurrentBuff);

            if (item != null && PerkTracking.SupportSpecialistWeapons(item))
            {
                int penBuff = (int)((PerkTracking.GetPenetrationBuffSupport() - 1f) * 100);
                TooltipLine PenBuff = new(Mod, "PenBuff", penBuff + "% " + Language.GetTextValue("Mods.InsurgencyWeapons.PerkSystem.CurrentPenBuff"))
                {
                    OverrideColor = Color.LightGreen,
                };
                tooltips.Add(PenBuff);
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (PerkTracking.CommandoWeapons(item))
                BuildTooltip(tooltips, (int)PerkSystem.Perks.Commando, PerkTracking.CommandoDamage, PerkTracking.CommandoKills);

            if (PerkTracking.SupportSpecialistWeapons(item))
                BuildTooltip(tooltips, (int)PerkSystem.Perks.SupportSpecialist, PerkTracking.SupportSpecialistDamage, PerkTracking.SupportSpecialistKills, item);

            if (PerkTracking.DemolitionsWeapons(item))
                BuildTooltip(tooltips, (int)PerkSystem.Perks.Demolitons, PerkTracking.DemolitionsDamage, PerkTracking.DemolitionsKills);

            if (PerkTracking.SharpshooterWeapons(item))
                BuildTooltip(tooltips, (int)PerkSystem.Perks.Sharpshooter, PerkTracking.SharpShooterDamage, PerkTracking.SharpShooterKills);

            if (item.ModItem is TwelveGaugeSlug || item.ModItem is TwelveGauge || item.ModItem is ShellBuck_Ball)
            {
                float penBuff = PerkTracking.GetPenetrationBuffSupport();
                int penValue = (int)Math.Round(3 * penBuff);
                TooltipLine penAmount = new(Mod, "penPowerAmount", PenBuff.Format(penValue));
                tooltips.Add(penAmount);
            }
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (Insurgency.AllWeapons.Contains(item.type))
            {
                if (PerkTracking.CommandoWeapons(item) && PerkTracking.Level[(int)PerkSystem.Perks.Commando] > 0)
                    damage *= 1f + PerkTracking.GetDamageMultPerLevel((int)PerkSystem.Perks.Commando);

                if (PerkTracking.SupportSpecialistWeapons(item) && PerkTracking.Level[(int)PerkSystem.Perks.SupportSpecialist] > 0)
                    damage *= 1f + PerkTracking.GetDamageMultPerLevel((int)PerkSystem.Perks.SupportSpecialist);

                if (PerkTracking.DemolitionsWeapons(item) && PerkTracking.Level[(int)PerkSystem.Perks.Demolitons] > 0)
                    damage *= 1f + PerkTracking.GetDamageMultPerLevel((int)PerkSystem.Perks.Demolitons);

                if (PerkTracking.SharpshooterWeapons(item) && PerkTracking.Level[(int)PerkSystem.Perks.Sharpshooter] > 0)
                    damage *= 1f + PerkTracking.GetDamageMultPerLevel((int)PerkSystem.Perks.Sharpshooter);

                if (InsurgencyModConfig.Instance.DamageScaling && (Insurgency.AllWeapons.Contains(item.type) || Insurgency.Grenades.Contains(item.type)))
                    damage *= Insurgency.WeaponScaling();
            }
        }
    }
}