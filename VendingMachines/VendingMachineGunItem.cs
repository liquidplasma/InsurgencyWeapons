using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.VendingMachines.Tiles;
using Microsoft.Xna.Framework.Graphics;

namespace InsurgencyWeapons.VendingMachines
{
    public class VendingMachineGunItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.DefaultToPlaceableTile(ModContent.TileType<VendingMachineGunsTile>());
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale = 0.5f;
            Texture2D texture = Item.MyTexture();
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