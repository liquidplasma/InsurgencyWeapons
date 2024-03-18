using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// PPSh-41 7.62x25mm Ammo
    /// </summary>
    public class Bullet76225 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 50;
            CraftStack = 71;
            Item.width = 7;
            Item.height = 18;
            Item.DefaultsToInsurgencyAmmo(9);
            base.SetDefaults();
        }
    }
}