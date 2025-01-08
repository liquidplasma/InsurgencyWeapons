namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// M72 LAW Rocket
    /// </summary>
    public class M72LAWRocket : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 68;
            MoneyCost = 75;
            CraftStack = 1;
            Item.DefaultsToInsurgencyAmmo(65);
            base.SetDefaults();
        }
    }
}