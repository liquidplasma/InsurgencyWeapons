using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.SubMachineGuns;

namespace InsurgencyWeapons.Items.Weapons.SubMachineGuns
{
    public class GreaseGun : SubMachineGun
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet45ACP>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 8;
            Item.width = 56;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 10;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<GreaseGunHeld>();
            MoneyCost = 170;
            base.SetDefaults();
        }
    }
}