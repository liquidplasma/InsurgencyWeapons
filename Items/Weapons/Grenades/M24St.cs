using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Grenades;

namespace InsurgencyWeapons.Items.Weapons.Grenades
{
    public class M24St : Grenade
    {
        public override void SetDefaults()
        {
            GrenadeType = ModContent.ProjectileType<M24StExplosive>();
            MoneyCost = 45;
            Item.damage = 162;
            Cook = Sounds.M24StCap;
            Spoon = Sounds.M24StRope;
            Throw = Sounds.M24StThrow;
            base.SetDefaults();
        }
    }
}