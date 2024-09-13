namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// C96 7.63x25mm Ammo
    /// </summary>
    public class Bullet76325 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 15;
            CraftStack = 30;
            Item.width = 7;
            Item.height = 18;
            Item.DefaultsToInsurgencyAmmo(4);
            base.SetDefaults();
        }
    }
}