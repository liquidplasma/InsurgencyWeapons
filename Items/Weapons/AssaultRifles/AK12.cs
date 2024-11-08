using InsurgencyWeapons.Projectiles.AssaultRifles;

namespace InsurgencyWeapons.Items.Weapons.AssaultRifles
{
    public class AK12 : AssaultRifle
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 5;
            Item.width = 76;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 11;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<AK12Held>();
            MoneyCost = 275;
            base.SetDefaults();
        }
    }
}