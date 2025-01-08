using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.BattleRifles;

namespace InsurgencyWeapons.Items.Weapons.BattleRifles
{
    public class M14EBR : BattleRifle
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet76251>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 5;
            Item.width = 102;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 15;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M14EBRHeld>();
            MoneyCost = 345;
            base.SetDefaults();
        }
    }
}