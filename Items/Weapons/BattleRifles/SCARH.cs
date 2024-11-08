using InsurgencyWeapons.Projectiles.BattleRifles;

namespace InsurgencyWeapons.Items.Weapons.BattleRifles
{
    public class SCARH : BattleRifle
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 6;
            Item.width = 82;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 13;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<SCARHHeld>();
            MoneyCost = 350;
            base.SetDefaults();
        }
    }
}