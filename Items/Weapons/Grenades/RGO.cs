using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Grenades;

namespace InsurgencyWeapons.Items.Weapons.Grenades
{
    public class RGO : Grenade
    {
        public override void SetDefaults()
        {
            GrenadeType = ModContent.ProjectileType<RGOExplosive>();
            MoneyCost = 65;
            Item.damage = 150;
            Cook = Sounds.MK2Pin;
            Spoon = Sounds.RGOSpoon;
            Throw = Sounds.MK2Throw;
            base.SetDefaults();
        }
    }
}