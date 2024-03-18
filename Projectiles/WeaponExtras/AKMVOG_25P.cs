using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Projectiles.WeaponExtras
{
    public class AKMVOG_25P : ModProjectile
    {
        private bool Exploded;
        private Player Player => Main.player[Projectile.owner];

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

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (InsurgencyModConfig.Instance.DamageScaling)
                modifiers.FinalDamage *= Insurgency.WeaponScaling();

            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            State = (int)Exploding.Ready;
        }

        public override void AI()
        {
            Projectile.FaceForward();
            if (Projectile.timeLeft < 6)
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
                Projectile.timeLeft = 6;
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
            if (Player.DistanceSQ(Projectile.Center) <= 128 * 128 && Collision.CanHitLine(Projectile.Center, 1, 1, Player.Center, 1, 1))
            {
                Player.HurtInfo grenadeSelfDamage = new()
                {
                    Dodgeable = true,
                    HitDirection = Projectile.Center.DirectionTo(Player.Center).X > 0f ? 1 : -1,
                    Damage = (int)(Projectile.damage * 0.9f),
                    DamageSource = PlayerDeathReason.ByProjectile(Player.whoAmI, Projectile.identity),
                    Knockback = 6f
                };
                Player.Hurt(grenadeSelfDamage);
            }
        }
    }
}