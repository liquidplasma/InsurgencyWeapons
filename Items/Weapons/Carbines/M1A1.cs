using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Carbines;

namespace InsurgencyWeapons.Items.Weapons.Carbines
{
    public class M1A1 : Carbine
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet76233>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 8;
            Item.width = 86;
            Item.height = 18;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 17;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M1A1Held>();
            MoneyCost = 260;
            base.SetDefaults();
        }
    }
}