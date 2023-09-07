using InsurgencyWeapons.Helpers;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 7.62x39mm Ammo
    /// </summary>
    internal class Bullet762 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 22;
            Item.DefaultsToInsurgencyAmmo(9);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 30, amountToCraft: 30);
        }
    }
}