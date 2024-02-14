using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// .45ACP
    /// </summary>
    internal class Bullet45ACP : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 50;
            CraftStack = 50;
            Item.width = 7;
            Item.height = 17;
            Item.DefaultsToInsurgencyAmmo(7);
            base.SetDefaults();
        }
    }
}