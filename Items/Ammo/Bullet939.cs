using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    internal class Bullet939 : AmmoItem
    {
        public override void SetDefaults()
        {
            Money = 20;
            CraftStack = 20;
            Item.width = 7;
            Item.height = 26;
            Item.DefaultsToInsurgencyAmmo(10);
            base.SetDefaults();
        }
    }
}