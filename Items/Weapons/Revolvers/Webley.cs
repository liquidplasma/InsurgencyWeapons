using InsurgencyWeapons.Projectiles.Revolvers;

namespace InsurgencyWeapons.Items.Weapons.Revolvers
{
    /// <summary>
    /// Webley Mk.VI .455 WMk.II
    /// </summary>
    public class Webley : Revolver
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 13;
            Item.width = 56;
            Item.height = 28;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 33;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<WebleyHeld>();
            MoneyCost = 160;
            base.SetDefaults();
        }
    }
}