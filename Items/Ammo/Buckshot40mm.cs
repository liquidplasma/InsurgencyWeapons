namespace InsurgencyWeapons.Items.Ammo
{
    internal class Buckshot40mm : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 46;
            MoneyCost = 40;
            CraftStack = 2;
            Item.DefaultsToInsurgencyAmmo(18);
            base.SetDefaults();
        }
    }
}