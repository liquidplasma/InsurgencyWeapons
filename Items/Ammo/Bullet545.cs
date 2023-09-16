using InsurgencyWeapons.Helpers;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 5.45x39mm Ammo
    /// </summary>
    internal class Bullet545 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 23;
            Item.DefaultsToInsurgencyAmmo(6);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 30, amountToCraft: 30);
        }
    }
}