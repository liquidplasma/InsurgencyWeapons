using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Carbines;

namespace InsurgencyWeapons.Items.Weapons.Carbines
{
    public class MK18 : Carbine
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet556>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 5;
            Item.width = 72;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 8;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<MK18Held>();
            MoneyCost = 220;
            base.SetDefaults();
        }
    }
}