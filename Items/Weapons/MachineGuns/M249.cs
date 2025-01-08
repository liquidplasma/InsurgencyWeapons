using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.MachineGuns;

namespace InsurgencyWeapons.Items.Weapons.MachineGuns
{
    public class M249 : LightMachineGun
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet556>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 5;
            Item.width = 84;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 11;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M249Held>();
            MoneyCost = 700;
            base.SetDefaults();
        }
    }
}