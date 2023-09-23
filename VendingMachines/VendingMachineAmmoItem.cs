﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.VendingMachines.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.VendingMachines
{
    internal class VendingMachineAmmoItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.DefaultToPlaceableTile(ModContent.TileType<VendingMachineAmmoTile>());
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale = 0.5f;
            Texture2D texture = TextureAssets.Item[Type].Value;
            Rectangle rect = texture.Bounds;
            ExtensionMethods.BetterEntityDraw(texture, Item.Top + new Vector2(0, -4f), rect, lightColor, rotation, texture.Size() / 2, scale, SpriteEffects.None);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(Insurgency.Money, 50)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}