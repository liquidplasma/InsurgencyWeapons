using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// M1 Garand 7.62x63mm Ammo
    /// </summary>
    public class Bullet3006 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 24;
            CraftStack = 16;
            Item.width = 7;
            Item.height = 36;
            Item.DefaultsToInsurgencyAmmo(16);
            base.SetDefaults();
        }
    }
}