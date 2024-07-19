namespace InsurgencyWeapons.Items.Ammo
{
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