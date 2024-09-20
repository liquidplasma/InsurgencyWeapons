using InsurgencyWeapons.Projectiles.Pistols;

namespace InsurgencyWeapons.Items.Weapons.Pistols
{
    public class M1911 : Pistol
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 12;
            Item.width = 30;
            Item.height = 20;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 17;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M1911Held>();
            MoneyCost = 110;
            base.SetDefaults();
        }
    }
}