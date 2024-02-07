using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 7.62x51mm Ammo
    /// </summary>
    internal class Bullet76251 : AmmoItem
    {
        public override void SetDefaults()
        {
            Money = 30;
            CraftStack = 20;
            Item.width = 7;
            Item.height = 31;
            Item.DefaultsToInsurgencyAmmo(11);
            base.SetDefaults();
        }
    }
}