using Microsoft.Xna.Framework;
using Terraria;

namespace InsurgencyWeapons.Projectiles.Grenades
{
    internal class RGOExplosive : GrenadeBase
    {
        private bool Contact => AITimer >= 45 && TileCollides == 0;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            FuseTime = 60 * 3.8f;
            Projectile.timeLeft = (int)FuseTime;
            base.SetDefaults();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Contact)
                Projectile.timeLeft = 3;

            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Contact)
                Projectile.timeLeft = 3;

            return base.OnTileCollide(oldVelocity);
        }
    }
}