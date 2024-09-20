using InsurgencyWeapons.Projectiles.Revolvers;

namespace InsurgencyWeapons.Items.Weapons.Revolvers
{
    public class M29 : Revolver
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 19;
            Item.width = 34;
            Item.height = 20;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 41;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M29Held>();
            MoneyCost = 190;
            base.SetDefaults();
        }
    }
}