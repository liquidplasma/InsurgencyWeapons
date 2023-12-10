using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.SubMachineGuns;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.SubMachineGuns
{
    /// <summary>
    /// Thompson M1928 .45ACP
    /// </summary>
    internal class ChicagoTypewriter : SubMachineGun
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.gunProj[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 6;
            Item.width = 74;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 10;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<ChicagoTypewriterHeld>();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(225);
        }
    }
}