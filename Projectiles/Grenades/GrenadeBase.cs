﻿using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.Grenades
{
    internal abstract class GrenadeBase : ModProjectile
    {
        public Player Player => Main.player[Projectile.owner];
        public float FuseTime { get; set; }
        public bool Moving => Projectile.velocity.Length() >= 0.5f;
        public bool HitOnce;

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
            Exploding
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Projectile.MyTexture();
            Rectangle rect = texture.Bounds;
            ExtensionMethods.BetterEntityDraw(texture, Projectile.Center, rect, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None);
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
                Projectile.velocity = (Projectile.Center.DirectionFrom(target.Center).RotatedByRandom(MathHelper.ToRadians(15)) * oldVel.Length());
                Projectile.velocity *= 0.7f;
                Projectile.netUpdate = true;
            }
            else
            {
                for (int i = 0; i < 72; i++)
                {
                    Vector2 dustPos = target.position + (target.Hitbox.Size() * Main.rand.NextFloat());
                    Dust effect = Dust.NewDustDirect(target.position, target.width, target.height, HelperStats.SmokeyDust);
                    effect.velocity =
                        Main.rand.NextBool()
                        ? dustPos.DirectionFrom(Projectile.Center) * 8f //true
                        : Utils.RandomVector2(Main.rand, -8f, 8f); //false

                    effect.scale = 3f * Main.rand.NextFloat();
                    effect.noGravity = Main.rand.NextBool();
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (State != (int)Exploded.Exploding)
            {
                modifiers.FinalDamage *= 0.067f;
            }
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void AI()
        {
            Projectile.RotateBasedOnVelocity(mult: 0.075f);
            AITimer++;
            if (AITimer > FuseTime / 18f)
                Projectile.velocity.Y += 0.24f;
            if (Projectile.timeLeft < 3)
            {
                State = (int)Exploded.Exploding;
                HitOnce = false;
                Projectile.Resize(240, 240);
                Projectile.alpha = 255;
                Projectile.velocity *= 0;
                Projectile.tileCollide = false;
                Projectile.netUpdate = true;
                HelperStats.SmokeGore(Projectile.GetSource_Death(), Projectile.Center, 35, 4);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Bounce(4);
            Projectile.velocity *= 0.7f;

            if (Moving)
                SoundEngine.PlaySound(Sounds.GrenadeTink, Projectile.Center);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(Sounds.GrenadeDetonation, Projectile.Center);
            if (Player.DistanceSQ(Projectile.Center) <= 128 * 128 && Collision.CanHitLine(Projectile.Center, 1, 1, Player.Center, 1, 1))
            {
                Player.HurtInfo greandeSelfDamage = new()
                {
                    Dodgeable = true,
                    HitDirection = Projectile.Center.DirectionTo(Player.Center).X > 0f ? 1 : -1,
                    Damage = (int)(Projectile.damage * 0.45f),
                    DamageSource = PlayerDeathReason.ByProjectile(Player.whoAmI, Projectile.identity),
                    Knockback = 6f
                };
                Player.Hurt(greandeSelfDamage);
            }
            base.Kill(timeLeft);
        }
    }
}