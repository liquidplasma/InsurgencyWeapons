using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 7.62x39mm Ammo
    /// </summary>
    public class Bullet762 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 30;
            CraftStack = 30;
            Item.width = 7;
            Item.height = 26;
            Item.DefaultsToInsurgencyAmmo(9);
            base.SetDefaults();
        }
    }
}