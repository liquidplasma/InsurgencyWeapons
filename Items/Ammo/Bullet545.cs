using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 5.45x39mm Ammo
    /// </summary>
    public class Bullet545 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 30;
            CraftStack = 30;
            Item.width = 7;
            Item.height = 27;
            Item.DefaultsToInsurgencyAmmo(6);
            base.SetDefaults();
        }
    }
}