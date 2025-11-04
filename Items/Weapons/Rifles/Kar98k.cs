using InsurgencyWeapons.Items;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Rifles;

namespace InsurgencyWeapons.Items.Weapons.Rifles
{
    public class Kar98k : Rifle
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet79257>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.crit = 15;
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 42;
            Item.width = 120;
            Item.height = 18;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 67;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<Kar98kHeld>();
            MoneyCost = 240;
            base.SetDefaults();
        }
    }
}