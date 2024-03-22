using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Lee-Enfield .303 Ammo
    /// </summary>
    public class Bullet303 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 25;
            CraftStack = 20;
            Item.width = 7;
            Item.height = 31;
            Item.DefaultsToInsurgencyAmmo(17);
            base.SetDefaults();
        }
    }
}