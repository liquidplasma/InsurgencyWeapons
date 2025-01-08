namespace InsurgencyWeapons.Items.Ammo
{
    internal class PZFaustRocket : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 58;
            MoneyCost = 48;
            CraftStack = 1;
            Item.DefaultsToInsurgencyAmmo(48);
            base.SetDefaults();
        }
    }
}