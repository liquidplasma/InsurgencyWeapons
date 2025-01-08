using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Revolvers;

namespace InsurgencyWeapons.Items.Weapons.Revolvers
{
    public class ColtPython : Revolver
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet357>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 34;
            Item.height = 18;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 30;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<ColtPythonHeld>();
            MoneyCost = 180;
            base.SetDefaults();
        }
    }
}