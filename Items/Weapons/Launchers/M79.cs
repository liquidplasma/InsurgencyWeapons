using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles;
using InsurgencyWeapons.Projectiles.Launchers;
using Terraria.ModLoader.IO;

namespace InsurgencyWeapons.Items.Weapons.Launchers
{
    internal class M79 : Launcher
    {
        private int M79SwitchTimer;

        private bool useBuck;

        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Grenade40mm>(), Type);
            AmmoItem.AddRelationShip(ModContent.ItemType<Buckshot40mm>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 40;
            Item.width = 82;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 151;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M79Held>();
            MoneyCost = 140;
            base.SetDefaults();
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(player, ref damage);
        }

        public override void HoldItem(Player player)
        {
            if (M79SwitchTimer > 0)
                M79SwitchTimer--;
            if (player.whoAmI == Main.myPlayer && WeaponHeldProjectile != 0 && player.ownedProjectileCounts[WeaponHeldProjectile] < 1)
            {
                Gun = Projectile.NewProjectileDirect(player.GetSource_ItemUse_WithPotentialAmmo(Item, Item.useAmmo), player.Center, Vector2.Zero, WeaponHeldProjectile, Item.damage, Item.knockBack, player.whoAmI);
                Gun.GetGlobalProjectile<ProjPerkTracking>().Perk = WeaponPerk;
                if (Gun.active && Gun.ModProjectile is WeaponBase changeAmmo)
                {
                    if (useBuck)
                    {
                        changeAmmo.AmmoType = ModContent.ItemType<Buckshot40mm>();
                    }
                    else
                    {
                        changeAmmo.AmmoType = ModContent.ItemType<Grenade40mm>();
                    }
                }
            }
            if (player.HasItem(ModContent.ItemType<Buckshot40mm>()) && Gun != null && Gun.active && Gun.ModProjectile is WeaponBase m79 && !m79.ReloadStarted && m79.MouseRightPressed && M79SwitchTimer == 0)
            {
                M79SwitchTimer = 90;
                m79.ReturnAmmo();
                m79.CurrentAmmo = 0;
                useBuck = useBuck != true;
                Gun.Kill();
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag[$"M79AmmoChoice{Item.ModItem?.Name}"] = useBuck;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey($"M79AmmoChoice{Item.ModItem?.Name}"))
                useBuck = tag.GetBool($"M79AmmoChoice{Item.ModItem?.Name}");
        }
    }
}