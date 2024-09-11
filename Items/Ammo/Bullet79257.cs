namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Gewehr 43 7.92x57mm Ammo
    /// </summary>
    public class Bullet79257 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 20;
            CraftStack = 20;
            Item.width = 7;
            Item.height = 28;
            Item.DefaultsToInsurgencyAmmo(14);
            base.SetDefaults();
        }
    }
}