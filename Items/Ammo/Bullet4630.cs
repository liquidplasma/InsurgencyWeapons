using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// MP7 4.6x30mm Ammo
    /// </summary>
    public class Bullet4630 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 45;
            CraftStack = 40;
            Item.width = 7;
            Item.height = 24;
            Item.DefaultsToInsurgencyAmmo(11);
            base.SetDefaults();
        }
    }
}