using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Pistols;

namespace InsurgencyWeapons.Items.Weapons.Pistols
{
    internal class VP70 : Pistol
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet919>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 4;
            Item.width = 42;
            Item.height = 28;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 14;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<VP70Held>();
            MoneyCost = 145;
            base.SetDefaults();
        }
    }
}