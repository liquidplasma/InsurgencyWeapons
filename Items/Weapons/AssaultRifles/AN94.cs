using InsurgencyWeapons.Projectiles.AssaultRifles;

namespace InsurgencyWeapons.Items.Weapons.AssaultRifles
{
    /// <summary>
    /// AN-94N + PK-AS 5.45x39mm
    /// </summary>
    internal class AN94 : AssaultRifle
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 6;
            Item.width = 80;
            Item.height = 28;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 11;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<AN94Held>();
            MoneyCost = 300;
        }
    }
}