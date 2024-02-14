using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Rifles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.Rifles
{
    internal class LeeEnfield : Rifle
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 36;
            Item.width = 78;
            Item.height = 18;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 37;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<EnfieldHeld>();
            MoneyCost = 270;
        }
    }
}