namespace InsurgencyWeapons.Items.Ammo
{
    public class RPGRocket : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 78;
            MoneyCost = 90;
            CraftStack = 1;
            Item.DefaultsToInsurgencyAmmo(110);
            base.SetDefaults();
        }
    }
}