namespace InsurgencyWeapons.Items.Ammo
{
    internal class Grenade40mm : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 62;
            MoneyCost = 50;
            CraftStack = 2;
            Item.DefaultsToInsurgencyAmmo(40);
            base.SetDefaults();
        }
    }
}