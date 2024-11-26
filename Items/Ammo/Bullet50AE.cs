namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Desert Eagle .50AE Ammo
    /// </summary>
    public class Bullet50AE : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 25;
            CraftStack = 14;
            Item.width = 7;
            Item.height = 24;
            Item.DefaultsToInsurgencyAmmo(21);
            base.SetDefaults();
        }
    }
}