using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.SubMachineGuns;

namespace InsurgencyWeapons.Items.Weapons.SubMachineGuns
{
    public class MP40 : SubMachineGun
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
            Item.useAnimation = Item.useTime = 7;
            Item.width = 66;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 10;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<MP40Held>();
            MoneyCost = 185;
            base.SetDefaults();
        }
    }
}