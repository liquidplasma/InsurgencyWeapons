using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Coach Gun Buck and Ball Ammo
    /// </summary>
    public class ShellBuck_Ball : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 20;
            CraftStack = 2;
            Item.DefaultsToInsurgencyAmmo(8);
            Item.width = 7;
            Item.height = 18;
            base.SetDefaults();
        }
    }
}