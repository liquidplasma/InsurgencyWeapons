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
            Money = 50;
            CraftStack = 8;
            Item.DefaultsToInsurgencyAmmo(6);
            Item.width = 7;
            Item.height = 18;
            base.SetDefaults();
        }
    }
}