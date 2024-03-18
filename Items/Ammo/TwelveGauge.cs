using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 12 Gauge Ammo
    /// </summary>
    public class TwelveGauge : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 50;
            CraftStack = 8;
            Item.DefaultsToInsurgencyAmmo(6);
            Item.width = 7;
            Item.height = 18;
            base.SetDefaults();
        }
    }
}