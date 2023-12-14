using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    internal class Bullet939 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 26;
            Item.DefaultsToInsurgencyAmmo(10);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 20, amountToCraft: 20);
        }
    }
}