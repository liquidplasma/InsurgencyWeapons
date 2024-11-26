namespace InsurgencyWeapons.Items.Ammo
{
    public class Bullet76233 : AmmoItem
    {
        /// <summary>
        /// M1A1 Paratrooper 7.62x33mm
        /// </summary>
        public override void SetDefaults()
        {
            MoneyCost = 20;
            CraftStack = 20;
            Item.width = 6;
            Item.height = 21;
            Item.DefaultsToInsurgencyAmmo(13);
            base.SetDefaults();
        }
    }
}