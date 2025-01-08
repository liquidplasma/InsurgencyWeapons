namespace InsurgencyWeapons.Items.Ammo
{
    public class AT4Rocket : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 36;
            MoneyCost = 70;
            CraftStack = 1;
            Item.DefaultsToInsurgencyAmmo(70);
            base.SetDefaults();
        }
    }
}