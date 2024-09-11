namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 9x19mm Ammo
    /// </summary>
    public class Bullet9x19 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 20;
            CraftStack = 30;
            Item.width = 7;
            Item.height = 17;
            Item.DefaultsToInsurgencyAmmo(3);
            base.SetDefaults();
        }
    }
}