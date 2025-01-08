using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.BattleRifles;

namespace InsurgencyWeapons.Items.Weapons.BattleRifles
{
    /// <summary>
    /// H&amp;K G3A3 7.62x51mm
    /// </summary>
    public class G3A3 : BattleRifle
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
            Item.useAnimation = Item.useTime = 7;
            Item.width = 80;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 11;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<G3A3Held>();
            MoneyCost = 335;
            base.SetDefaults();
        }
    }
}