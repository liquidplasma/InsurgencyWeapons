using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Carbines;

namespace InsurgencyWeapons.Items.Weapons.Carbines
{
    public class SKS : Carbine
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet762>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 78;
            Item.height = 18;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 20;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<SKSHeld>();
            MoneyCost = 270;
            base.SetDefaults();
        }
    }
}