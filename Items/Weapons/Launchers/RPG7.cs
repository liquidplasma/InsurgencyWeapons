using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Launchers;

namespace InsurgencyWeapons.Items.Weapons.Launchers
{
    public class RPG7 : Launcher
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<RPGRocket>(), Type);
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
            Item.damage = 240;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<RPG7Held>();
            MoneyCost = 400;
            base.SetDefaults();
        }
    }
}