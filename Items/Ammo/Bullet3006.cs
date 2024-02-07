using InsurgencyWeapons.Helpers;
using Terraria;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// M1 Garand 7.62x63mm Ammo
    /// </summary>
    internal class Bullet3006 : AmmoItem
    {
        public override void SetDefaults()
        {
            Money = 15;
            CraftStack = 8;
            Item.width = 7;
            Item.height = 36;
            Item.DefaultsToInsurgencyAmmo(14);
            base.SetDefaults();
        }
    }
}