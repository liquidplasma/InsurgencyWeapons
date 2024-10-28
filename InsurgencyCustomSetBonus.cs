using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using System.Collections.Generic;
using Terraria.Localization;

namespace InsurgencyWeapons
{
    public class InsurgencyCustomSetBonusModPlayer : ModPlayer
    {
        public bool
            revolverSet,
            swatHelmet;

        /// <summary>
        /// This player has ItemID.SniperScope equipped
        /// </summary>
        public bool sniperScope;

        public override void ResetEffects()
        {
            revolverSet = false;
            swatHelmet = false;
            sniperScope = false;
            base.ResetEffects();
        }
    }

    public class InsurgencyCustomSetBonus : GlobalItem
    {
        private InsurgencyCustomSetBonusModPlayer GetSetBonusModPlayer(Player player) => player.GetModPlayer<InsurgencyCustomSetBonusModPlayer>();

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.ObsidianHelm && body.type == ItemID.ObsidianShirt && legs.type == ItemID.ObsidianPants)
                return "INSObsidianRevolver";
            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "INSObsidianRevolver")
            {
                GetSetBonusModPlayer(player).revolverSet = true;
                player.setBonus += "\n" + Language.GetTextValue("Mods.InsurgencyWeapons.SetBonuses.RevolverSet");
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            switch (item.type)
            {
                case ItemID.SWATHelmet:
                    {
                        GetSetBonusModPlayer(player).swatHelmet = true;
                        break;
                    }
                case ItemID.SniperScope:
                    {
                        GetSetBonusModPlayer(player).sniperScope = true;
                        break;
                    }
            }
            base.UpdateEquip(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.SWATHelmet:
                    {
                        TooltipLine swatHelmet = new(Mod, "swatHelmetText", Language.GetTextValue("Mods.InsurgencyWeapons.SetBonuses.SwatHelmet"));
                        tooltips.Add(swatHelmet);
                        break;
                    }
                case ItemID.SniperScope:
                    {
                        TooltipLine sniperScope = new(Mod, "sniperScopeHelmet", Language.GetTextValue("Mods.InsurgencyWeapons.SetBonuses.SniperScope"));
                        tooltips.Add(sniperScope);
                        break;
                    }
            }
            base.ModifyTooltips(item, tooltips);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if ((Insurgency.Carbines.Contains(item.type) || Insurgency.AssaultRifles.Contains(item.type)) && GetSetBonusModPlayer(player).swatHelmet)
                damage *= 1.33f;
            if ((Insurgency.Pistols.Contains(item.type) || Insurgency.Revolvers.Contains(item.type)) && GetSetBonusModPlayer(player).revolverSet)
                damage *= 2f;
            if (item.ModItem is WeaponUtils weapon && GetSetBonusModPlayer(player).sniperScope)
            {
                if (weapon.WeaponPerk == ((int)PerkSystem.Perks.Sharpshooter))
                {
                    damage *= 1.4f;
                }
            }
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if ((Insurgency.Carbines.Contains(item.type) || Insurgency.AssaultRifles.Contains(item.type)) && GetSetBonusModPlayer(player).swatHelmet)
                crit += 15;
            if ((Insurgency.Pistols.Contains(item.type) || Insurgency.Revolvers.Contains(item.type)) && GetSetBonusModPlayer(player).revolverSet)
                crit += 25;
            if (item.ModItem is WeaponUtils weapon && GetSetBonusModPlayer(player).sniperScope)
            {
                if (weapon.WeaponPerk == ((int)PerkSystem.Perks.Sharpshooter))
                {
                    crit += 20;
                }
            }
        }
    }
}