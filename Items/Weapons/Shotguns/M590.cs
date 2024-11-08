using InsurgencyWeapons.Projectiles.Shotguns;

namespace InsurgencyWeapons.Items.Weapons.Shotguns
{
    /// <summary>
    /// Mossberg M590 12 Gauge
    /// </summary>
    public class M590 : Shotgun
    {
        public override void SetDefaults()
        {
            Item.knockBack = 6f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 48;
            Item.width = 78;
            Item.height = 20;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 16;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M590Held>();
            MoneyCost = 285;
            base.SetDefaults();
        }
    }
}