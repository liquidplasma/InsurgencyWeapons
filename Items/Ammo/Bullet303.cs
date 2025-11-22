using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// .303 Ammo 7.7×56mmR
    /// </summary>
    public class Bullet303 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 25;
            CraftStack = 20;
            Item.width = 7;
            Item.height = 31;
            Item.DefaultsToInsurgencyAmmo(11);
            base.SetDefaults();
        }
    }
}