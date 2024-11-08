using InsurgencyWeapons.Projectiles.BattleRifles;

namespace InsurgencyWeapons.Items.Weapons.BattleRifles
{
    /// <summary>
    /// FG-42 7.92x57mm
    /// </summary>
    public class FG42 : BattleRifle
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 4;
            Item.width = 78;
            Item.height = 22;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 14;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<FG42Held>();
            MoneyCost = 360;
            base.SetDefaults();
        }
    }
}