using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Python .357 Magnum Ammo
    /// </summary>
    public class Bullet357 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 15;
            CraftStack = 12;
            Item.width = 7;
            Item.height = 21;
            Item.DefaultsToInsurgencyAmmo(20);
            base.SetDefaults();
        }
    }
}