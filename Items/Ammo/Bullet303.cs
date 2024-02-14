using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Lee-Enfield .303 Ammo
    /// </summary>
    internal class Bullet303 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 20;
            CraftStack = 10;
            Item.width = 7;
            Item.height = 31;
            Item.DefaultsToInsurgencyAmmo(17);
            base.SetDefaults();
        }
    }
}