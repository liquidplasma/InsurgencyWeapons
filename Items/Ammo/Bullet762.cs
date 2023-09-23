using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 7.62x39mm Ammo
    /// </summary>
    internal class Bullet762 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 26;
            Item.DefaultsToInsurgencyAmmo(9);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 30, amountToCraft: 30);
        }
    }
}