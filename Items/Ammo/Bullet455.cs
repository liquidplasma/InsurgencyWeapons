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
            MoneyCost = 10;
            CraftStack = 6;
            Item.width = 7;
            Item.height = 18;
            Item.DefaultsToInsurgencyAmmo(13);
            base.SetDefaults();
        }
    }
}