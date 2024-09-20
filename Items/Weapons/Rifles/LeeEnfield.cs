using InsurgencyWeapons.Projectiles.Rifles;

namespace InsurgencyWeapons.Items.Weapons.Rifles
{
    public class LeeEnfield : Rifle
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 36;
            Item.width = 78;
            Item.height = 18;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 37;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<EnfieldHeld>();
            MoneyCost = 270;
            base.SetDefaults();
        }
    }
}