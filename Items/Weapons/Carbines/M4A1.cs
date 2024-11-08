using InsurgencyWeapons.Projectiles.Carbines;

namespace InsurgencyWeapons.Items.Weapons.Carbines
{
    public class M4A1 : Carbine
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 5;
            Item.width = 78;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 9;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M4A1Held>();
            MoneyCost = 245;
            base.SetDefaults();
        }
    }
}