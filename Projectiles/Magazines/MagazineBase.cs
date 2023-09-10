using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.Magazines
{
    internal abstract class MagazineBase : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 3600;
            Projectile.light = 0;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void AI()
        {
            if (Projectile.penetrate > 1)
            {
                Projectile.alpha -= 16;
                Projectile.velocity.Y += 0.1f;
                Projectile.RotateBasedOnVelocity();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate == 1)
            {
                Projectile.tileCollide = false;
                Projectile.velocity *= 0;
            }
            else
            {
                Projectile.Bounce(60);
                Projectile.velocity *= 0.5f;
            }

            return false;
        }
    }
}