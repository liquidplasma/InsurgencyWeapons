using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Projectiles.WeaponMagazines
{
    public abstract class MagazineBase : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.penetrate = 5;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1800;
            Projectile.light = 0;
        }

        public override bool? CanCutTiles()
        {
            return false;
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

                if (Projectile.velocity.Y >= 12f)
                    Projectile.velocity = Projectile.oldVelocity;

                Projectile.RotateBasedOnVelocity();
            }
            else if (Projectile.penetrate == 1)
            {
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.penetrate > 1)
            {
                Projectile.Bounce(60);
                Projectile.velocity *= 0.5f;
            }
            Projectile.penetrate--;
            return false;
        }
    }
}