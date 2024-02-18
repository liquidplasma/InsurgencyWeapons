using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// StG-44 7.92x33mm Ammo
    /// </summary>
    internal class Bullet792 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 30;
            CraftStack = 30;
            Item.width = 7;
            Item.height = 24;
            Item.DefaultsToInsurgencyAmmo(8);
            base.SetDefaults();
        }
    }
}