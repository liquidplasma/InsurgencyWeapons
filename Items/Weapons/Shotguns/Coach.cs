using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Shotguns;

namespace InsurgencyWeapons.Items.Weapons.Shotguns
{
    /// <summary>
    /// Coach Gun Buck and Ball
    /// </summary>
    public class Coach : Shotgun
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<TwelveGauge>(), Type);
            AmmoItem.AddRelationShip(ModContent.ItemType<TwelveGaugeSlug>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 6f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 12;
            Item.width = 76;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 23;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<CoachHeld>();
            MoneyCost = 295;
            base.SetDefaults();
        }
    }
}