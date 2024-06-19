using InsurgencyWeapons.Projectiles.Shotguns;
using Terraria.ModLoader.IO;

namespace InsurgencyWeapons.Items.Weapons.Shotguns
{
    /// <summary>
    /// Ithaca M37 12 Gauge
    /// </summary>
    public class Ithaca : Shotgun
    {
        public override void SetDefaults()
        {
            Item.knockBack = 6f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 45;
            Item.width = 82;
            Item.height = 18;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 14;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<IthacaHeld>();
            MoneyCost = 275;
            base.SetDefaults();
        }
    }
}