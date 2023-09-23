using InsurgencyWeapons.Helpers;
using Terraria;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Python .357 Magnum Ammo
    /// </summary>
    internal class Bullet357 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 21;
            Item.DefaultsToInsurgencyAmmo(14);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 15, amountToCraft: 6);
        }
    }
}