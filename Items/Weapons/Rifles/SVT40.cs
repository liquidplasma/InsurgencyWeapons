using InsurgencyWeapons.Projectiles.Rifles;

namespace InsurgencyWeapons.Items.Weapons.Rifles
{
    /// <summary>
    /// SVT-40 7.62x54mmR
    /// </summary>
    public class SVT40 : Rifle
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 15;
            Item.width = 80;
            Item.height = 16;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 18;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<SVT40Held>();
            MoneyCost = 290;
            base.SetDefaults();
        }
    }
}