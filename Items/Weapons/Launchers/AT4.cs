using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Launchers;

namespace InsurgencyWeapons.Items.Weapons.Launchers
{
    public class AT4 : Launcher
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<AT4Rocket>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 40;
            Item.width = 108;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 200;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<AT4Held>();
            MoneyCost = 450;
            base.SetDefaults();
        }
    }
}