using InsurgencyWeapons.Projectiles.SniperRifles;

namespace InsurgencyWeapons.Items.Weapons.SniperRifles
{
    public class M40A1 : SniperRifle
    {
        public override void SetDefaults()
        {
            Item.crit = 17;
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 40;
            Item.width = 80;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 57;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M40A1Held>();
            MoneyCost = 305;
        }
    }
}