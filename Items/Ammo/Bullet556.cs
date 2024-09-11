namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 5.56x45mm Ammo
    /// </summary>
    public class Bullet556 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 30;
            CraftStack = 30;
            Item.width = 7;
            Item.height = 27;
            Item.DefaultsToInsurgencyAmmo(7);
            base.SetDefaults();
        }
    }
}