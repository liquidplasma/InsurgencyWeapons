using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Model 29 .44 Magnum Ammo
    /// </summary>
    internal class Bullet44 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 19;
            Item.DefaultsToInsurgencyAmmo(18);
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 20, amountToCraft: 6);
        }
    }
}