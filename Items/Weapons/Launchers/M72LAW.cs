using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Launchers;

namespace InsurgencyWeapons.Items.Weapons.Launchers
{
    public class M72LAW : Launcher
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<M72LAWRocket>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 40;
            Item.width = 102;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 275;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M72LAWHeld>();
            MoneyCost = 425;
            base.SetDefaults();
        }
    }
}