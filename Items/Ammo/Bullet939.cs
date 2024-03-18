using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    public class Bullet939 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 20;
            CraftStack = 20;
            Item.width = 7;
            Item.height = 26;
            Item.DefaultsToInsurgencyAmmo(10);
            base.SetDefaults();
        }
    }
}