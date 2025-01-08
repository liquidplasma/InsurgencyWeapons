using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.MachineGuns;

namespace InsurgencyWeapons.Items.Weapons.MachineGuns
{
    public class RPK : LightMachineGun
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet762>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 6;
            Item.width = 84;
            Item.height = 22;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 14;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<RPKHeld>();
            MoneyCost = 485;
            base.SetDefaults();
        }
    }
}