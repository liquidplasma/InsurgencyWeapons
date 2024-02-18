using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Grenades;

namespace InsurgencyWeapons.Items.Weapons.Grenades
{
    internal class MK2 : Grenade
    {
        public override void SetDefaults()
        {
            GrenadeType = ModContent.ProjectileType<MK2Explosive>();
            MoneyCost = 50;
            Item.damage = 160;
            Cook = Sounds.MK2Pin;
            Spoon = Sounds.MK2Spoon;
            Throw = Sounds.MK2Throw;
            base.SetDefaults();
        }
    }
}