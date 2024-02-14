using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Revolvers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.Revolvers
{
    internal class ColtPython : Revolver
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 34;
            Item.height = 18;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 19;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<ColtPythonHeld>();
            MoneyCost = 180;
        }
    }
}