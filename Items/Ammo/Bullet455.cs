using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Webley .455 WMk.II
    /// </summary>
    public class Bullet455 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 15;
            CraftStack = 12;
            Item.width = 7;
            Item.height = 18;
            Item.DefaultsToInsurgencyAmmo(13);
            base.SetDefaults();
        }
    }
}