using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// PPSh-41 7.62x25mm Ammo
    /// </summary>
    internal class Bullet76225 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 18;
            Item.DefaultsToInsurgencyAmmo(9);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 50, amountToCraft: 71);
        }
    }
}