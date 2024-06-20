using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Rifles;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;

namespace InsurgencyWeapons.Projectiles.WeaponMagazines
{
    public abstract class MagazineBase : ModProjectile
    {
        private static int[] IgnoredDraws = {
            ModContent.ProjectileType<M1GarandEnbloc>(),
            ModContent.ProjectileType<EnfieldBlock>(),
        };
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

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ModProjectile is MagazineBase)
            {
                if (IgnoredDraws.Contains(Projectile.type))
                    return base.PreDraw(ref lightColor);

                Texture2D texture = Projectile.MyTexture();
                Rectangle rect = texture.Bounds;
                BetterEntityDraw(texture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, Projectile.scale, SpriteEffects.None);
                return false;
            }
            return base.PreDraw(ref lightColor);
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