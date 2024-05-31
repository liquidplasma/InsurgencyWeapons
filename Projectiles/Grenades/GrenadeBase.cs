using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace InsurgencyWeapons.Projectiles.Grenades
{
    public abstract class GrenadeBase : ModProjectile
    {
        public Player Player => Main.player[Projectile.owner];

        /// <summary>
        /// In seconds
        /// </summary>
        public int FuseTime { get; set; }

        public bool NPCProj { get; set; }
        public bool Moving => Projectile.velocity.Length() >= 0.5f;
        public bool HitOnce;

        public int TileCollides;

        public int AITimer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public int State
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public enum Exploded
        {
            Not,

            /// <summary>
            /// Projectile timeleft is less than 6
            /// </summary>
            Exploding
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = HelperStats.SecondsToTick(FuseTime);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Projectile.MyTexture();
            Rectangle rect = texture.Bounds;
            BetterEntityDraw(texture, Projectile.Center, rect, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (HitOnce || (Moving && State == ((int)Exploded.Exploding)))
                return false;
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (State != (int)Exploded.Exploding)
            {
                SoundEngine.PlaySound(Sounds.GrenadeTink, Projectile.Center);
                HitOnce = true;
                Vector2 oldVel = Projectile.velocity;
                Projectile.velocity = Projectile.Center.DirectionFrom(target.Center).RotatedByRandom(MathHelper.ToRadians(15)) * oldVel.Length();
                Projectile.velocity *= 0.18f;
                Projectile.netUpdate = true;
            }
            else
            {
                if (!target.active)
                {
                    for (int i = 0; i < 72; i++)
                    {
                        Vector2 dustPos = target.position + (target.Hitbox.Size() * Main.rand.NextFloat());
                        Dust effect = Dust.NewDustDirect(target.position, target.width, target.height, HelperStats.SmokeyDust);
                        effect.velocity =
                            Main.rand.NextBool()
                            ? dustPos.DirectionFrom(target.Center) * 8f //true
                            : Utils.RandomVector2(Main.rand, -8f, 8f); //false

                        effect.scale = 3f * Main.rand.NextFloat();
                        effect.noGravity = Main.rand.NextBool();
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (State != (int)Exploded.Exploding)
            {
                modifiers.FinalDamage *= 0.5f;
            }
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void AI()
        {
            Projectile.RotateBasedOnVelocity(mult: 0.075f);
            AITimer++;
            if (AITimer > FuseTime / 18f)
            {
                Projectile.velocity.Y += 0.24f;
                if (Projectile.velocity.Y >= 10f)
                    Projectile.velocity.Y = Projectile.oldVelocity.Y;
            }
            if (Projectile.timeLeft < 6)
            {
                State = (int)Exploded.Exploding;
                HitOnce = false;
                Projectile.Resize(240, 240);
                Projectile.alpha = 255;
                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = -1;
                Projectile.netUpdate = true;
                HelperStats.SmokeGore(Projectile.GetSource_Death(), Projectile.Center, 9, 4);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            TileCollides++;
            Projectile.Bounce(4);
            Projectile.velocity *= 0.7f;

            if (Moving)
                SoundEngine.PlaySound(Sounds.GrenadeTink, Projectile.Center);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(Sounds.GrenadeDetonation with { Volume = 0.4f, MaxInstances = 0 }, Projectile.Center);
            if (!NPCProj && Player.DistanceSQ(Projectile.Center) <= 128 * 128 && Collision.CanHitLine(Projectile.Center, 1, 1, Player.Center, 1, 1))
            {
                Player.HurtInfo grenadeSelfDamage = new()
                {
                    Dodgeable = true,
                    HitDirection = Projectile.Center.DirectionTo(Player.Center).X > 0f ? 1 : -1,
                    Damage = (int)(Projectile.damage * 0.45f),
                    DamageSource = PlayerDeathReason.ByProjectile(Player.whoAmI, Projectile.identity),
                    Knockback = 6f
                };
                Player.Hurt(grenadeSelfDamage);
            }
            for (int i = 0; i < Main.rand.Next(30, 60); i++)
            {
                Vector2 random = Utils.NextVector2Circular(Main.rand, 4, 4);
                if (Player.whoAmI == Main.myPlayer)
                {
                    Projectile shrapnel = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, random, ModContent.ProjectileType<Shrapnel>(), (int)(Projectile.damage * 0.1f), 1f);
                    shrapnel.GetGlobalProjectile<ProjPerkTracking>().ShotFromInsurgencyWeapon = true;
                }
            }
            base.OnKill(timeLeft);
        }
    }
}