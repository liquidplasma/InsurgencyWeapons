using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Casings
{
    internal class Shells : MagazineBase
    {
        private SoundStyle Tink => new("InsurgencyWeapons/Sounds/Weapons/Shells/sshell", 6)
        {
            Volume = 0.4f,
        };

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.penetrate = 8;
            Projectile.timeLeft = 1350;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(Tink, Projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }
    }
}