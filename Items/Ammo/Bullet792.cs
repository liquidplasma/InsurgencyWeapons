using InsurgencyWeapons.Helpers;
using Terraria;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// StG-44 7.92x33mm Ammo
    /// </summary>
    internal class Bullet792 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 24;
            Item.DefaultsToInsurgencyAmmo(8);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 30, amountToCraft: 30);
        }
    }
}