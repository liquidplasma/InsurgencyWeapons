using InsurgencyWeapons.Projectiles.AssaultRifles;

namespace InsurgencyWeapons.Items.Weapons.AssaultRifles
{
    /// <summary>
    /// IMI GALIL ARM 5.56x45mm
    /// </summary>
    public class Galil : AssaultRifle
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 6;
            Item.width = 76;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 10;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<GalilHeld>();
            MoneyCost = 260;
            base.SetDefaults();
        }
    }
}