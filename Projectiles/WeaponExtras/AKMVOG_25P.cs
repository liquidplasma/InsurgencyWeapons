using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.WeaponExtras
{
    internal class AKMVOG_25P : ModProjectile
    {
        private bool Exploded;

        private enum Exploding
        {
            Not = 0,
            Ready = 1,
        }

        private int Delay
        {
            get => (int)Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        private int State
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 900;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            State = (int)Exploding.Ready;
            if (target.CanBeChasedBy(this) && !target.boss)
            {
                target.velocity += target.Center.DirectionFrom(Projectile.Center) * 4f;
                target.netUpdate = true;
            }
        }

        public override void AI()
        {
            Projectile.FaceForward();
            if (Projectile.timeLeft < 3)
            {
                Projectile.Resize(384, 384);
                Projectile.velocity *= 0f;
                Projectile.alpha = 255;
                Projectile.tileCollide = false;
                HelperStats.SmokeGore(Projectile.GetSource_Death(), Projectile.Center, 35, 4);
            }
            else
            {
                Projectile.alpha -= 15;
            }

            Delay++;
            if (Delay >= 6)
            {
                if (Delay >= 30)
                    Projectile.velocity.Y += 0.075f;
                HelperStats.SmokeyTrail(Projectile.Center, Projectile.oldVelocity);
            }
            if (State == (int)Exploding.Ready && !Exploded)
            {
                Projectile.timeLeft = 3;
                Exploded = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            State = (int)Exploding.Ready;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(Sounds.GrenadeDetonation with { Volume = 0.4f, MaxInstances = 0 }, Projectile.Center);
        }
    }
}