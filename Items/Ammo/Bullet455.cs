using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Webley .455 WMk.II
    /// </summary>
    internal class Bullet455 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 18;
            Item.DefaultsToInsurgencyAmmo(13);
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            //this.RegisterINS2RecipeAmmo(money: 10, amountToCraft: 6);
        }
    }
}