using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.MachineGuns;

namespace InsurgencyWeapons.Items.Weapons.MachineGuns
{
    public class PKM : LightMachineGun
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet76254R>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 7;
            Item.width = 80;
            Item.height = 22;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 16;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<PKMHeld>();
            MoneyCost = 950;
            base.SetDefaults();
        }
    }
}