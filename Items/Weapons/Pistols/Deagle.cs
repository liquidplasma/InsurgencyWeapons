using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Pistols;

namespace InsurgencyWeapons.Items.Weapons.Pistols
{
    public class Deagle : Pistol
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet50AE>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 14;
            Item.width = 50;
            Item.height = 28;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 35;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<DeagleHeld>();
            MoneyCost = 200;
            base.SetDefaults();
        }
    }
}