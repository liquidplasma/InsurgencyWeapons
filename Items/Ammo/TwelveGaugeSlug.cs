namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 12 Gauge Slug Ammo
    /// </summary>
    public class TwelveGaugeSlug : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 50;
            CraftStack = 8;
            Item.DefaultsToInsurgencyAmmo(54);
            Item.width = 7;
            Item.height = 18;
            base.SetDefaults();
        }
    }
}