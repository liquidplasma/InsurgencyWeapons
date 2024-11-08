using InsurgencyWeapons.Projectiles.Pistols;

namespace InsurgencyWeapons.Items.Weapons.Pistols
{
    /// <summary>
    /// Mauser C96 7.63x25mm
    /// </summary>
    public class C96 : Pistol
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 30;
            Item.height = 20;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 21;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<C96Held>();
            MoneyCost = 110;
            base.SetDefaults();
        }
    }
}