using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.MachineGuns;

namespace InsurgencyWeapons.Items.Weapons.MachineGuns
{
    public class Lewis : LightMachineGun
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet303>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 7;
            Item.width = 110;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 15;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<LewisHeld>();
            MoneyCost = 400;
            base.SetDefaults();
        }
    }
}