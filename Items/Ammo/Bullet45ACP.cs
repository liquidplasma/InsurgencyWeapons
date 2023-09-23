using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// .45ACP
    /// </summary>
    internal class Bullet45ACP : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 17;
            Item.DefaultsToInsurgencyAmmo(7);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 50, amountToCraft: 50);
        }
    }
}