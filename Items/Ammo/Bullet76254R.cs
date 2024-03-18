using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 7.62x54mmR Ammo
    /// </summary>
    public class Bullet76254R : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 15;
            CraftStack = 10;
            Item.width = 7;
            Item.height = 33;
            Item.DefaultsToInsurgencyAmmo(18);
            base.SetDefaults();
        }
    }
}