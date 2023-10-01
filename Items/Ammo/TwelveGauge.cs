using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 12 Gauge Ammo
    /// </summary>
    internal class TwelveGauge : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.DefaultsToInsurgencyAmmo(6);
            Item.width = 7;
            Item.height = 18;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 50, amountToCraft: 8);
        }
    }
}