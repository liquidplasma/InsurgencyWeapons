namespace InsurgencyWeapons.Items.Ammo
{
    public class PanzerschreckRocket : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 6;
            Item.height = 48;
            MoneyCost = 80;
            CraftStack = 1;
            Item.DefaultsToInsurgencyAmmo(144);
            base.SetDefaults();
        }
    }
}