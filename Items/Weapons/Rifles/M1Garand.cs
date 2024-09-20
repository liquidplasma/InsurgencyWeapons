using InsurgencyWeapons.Projectiles.Rifles;

namespace InsurgencyWeapons.Items.Weapons.Rifles
{
    public class M1Garand : Rifle
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 66;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 23;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M1GarandHeld>();
            MoneyCost = 255;
            base.SetDefaults();
        }
    }
}