using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Revolvers;

namespace InsurgencyWeapons.Items.Weapons.Revolvers
{
    public class M29 : Revolver
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet44>(), Type);
            base.SetStaticDefaults();
        }

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
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M29Held>();
            MoneyCost = 190;
            base.SetDefaults();
        }
    }
}