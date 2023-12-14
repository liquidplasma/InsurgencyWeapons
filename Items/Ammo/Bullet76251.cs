using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 7.62x51mm Ammo
    /// </summary>
    internal class Bullet76251 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 31;
            Item.DefaultsToInsurgencyAmmo(11);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 30, amountToCraft: 20);
        }
    }
}