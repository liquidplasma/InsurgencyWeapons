using InsurgencyWeapons.Projectiles.Shotguns;

namespace InsurgencyWeapons.Items.Weapons.Shotguns
{
    /// <summary>
    /// Coach Gun Buck and Ball
    /// </summary>
    public class Coach : Shotgun
    {
        public override void SetDefaults()
        {
            Item.knockBack = 6f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 12;
            Item.width = 76;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 23;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<CoachHeld>();
            MoneyCost = 295;
            base.SetDefaults();
        }

        public override void HoldItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && WeaponHeldProjectile != 0 && player.ownedProjectileCounts[WeaponHeldProjectile] < 1)
            {
                Projectile Gun = Projectile.NewProjectileDirect(player.GetSource_ItemUse_WithPotentialAmmo(Item, Item.useAmmo), player.Center, Vector2.Zero, WeaponHeldProjectile, Item.damage, Item.knockBack, player.whoAmI);
                Gun.GetGlobalProjectile<ProjPerkTracking>().Perk = WeaponPerk;
            }
        }
    }
}