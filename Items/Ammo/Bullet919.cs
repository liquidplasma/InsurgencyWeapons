namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 9x19mm Ammo
    /// </summary>
    public class Bullet919 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 15;
            CraftStack = 30;
            Item.width = 7;
            Item.height = 17;
            Item.DefaultsToInsurgencyAmmo(3);
            base.SetDefaults();
        }
    }
}