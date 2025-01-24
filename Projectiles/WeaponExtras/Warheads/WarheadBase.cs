using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace InsurgencyWeapons.Projectiles.WeaponExtras.Warheads
{
    public abstract class WarheadBase : ModProjectile
    {
        public ref float FlyTime => ref Projectile.ai[2];

        public int State
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public int FlyTimeSeconds => (int)Math.Round(FlyTime / 60f);
        private Item HeldItem => Player.HeldItem;
        public Player Player => Main.player[Projectile.owner];

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
            Projectile.width = Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.Opacity = 0f;
            Projectile.extraUpdates += 1;
            if (this is AT4Warhead)
                Projectile.extraUpdates = 0;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.Opacity <= 0.2f)
                return false;

            if (this is PanzerfaustWarhead)
            {
                Texture2D texture = Projectile.MyTexture();
                Rectangle rect = texture.Frame(verticalFrames: 2, frameY: Projectile.frame);
                BetterEntityDraw(texture, Projectile.Center, rect, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None);
                return false;
            }
            if (Projectile.timeLeft > 6)
            {
                Texture2D texture = Projectile.MyTexture();
                Rectangle rect = texture.Bounds;
                BetterEntityDraw(texture, Projectile.Center, rect, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None);
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Projectile.timeLeft = 6;
            float mult = 1f;
            PerkSystem PerkTracking = Player.GetModPlayer<PerkSystem>();
            if (PerkTracking.DemolitionsWeapons(HeldItem) && PerkTracking.Level[(int)PerkSystem.Perks.Demolitons] > 0)
                mult += PerkTracking.GetDamageMultPerLevel((int)PerkSystem.Perks.Demolitons);

            if (InsurgencyModConfig.Instance.DamageScaling)
                mult *= Insurgency.WeaponScaling();
            modifiers.FinalDamage *= mult;
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override bool PreAI()
        {
            if (FlyTimeSeconds >= 15)
                Projectile.Kill();
            return base.PreAI();
        }

        public override void AI()
        {
            Projectile.FaceForward();
            FlyTime++;
            if (FlyTime >= 60f)
            {
                Projectile.velocity.Y += 0.015f;
                if (Projectile.velocity.Y >= 16f)
                    Projectile.velocity.Y = Projectile.oldVelocity.Y;
            }
            if (Projectile.timeLeft <= 6)
            {
                State = (int)Exploded.Exploding;
                Vector2 explosionRadius = new(360, 360);
                if (this is PanzerfaustWarhead)
                    explosionRadius *= 0.9f;
                Projectile.Resize((int)explosionRadius.X, (int)explosionRadius.Y);
                Projectile.alpha = 255;
                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = -1;
                Projectile.netUpdate = true;
                for (int i = 0; i < 6; i++)
                    HelperStats.SmokeGore(Projectile.GetSource_Death(), Projectile.Center, 9, 9);
            }
            if (Projectile.Opacity <= 1f)
                Projectile.Opacity += 0.01f;

            if (Projectile.Opacity >= 0.1f)
                HelperStats.SmokeyTrail(Projectile.Center, Projectile.velocity);

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.timeLeft = 6;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.wet)
                SoundEngine.PlaySound(Sounds.WetRocketDetonation with { Volume = 0.4f, MaxInstances = 0 }, Projectile.Center);
            else
                SoundEngine.PlaySound(Sounds.RocketDetonation with { Volume = 0.4f, MaxInstances = 0 }, Projectile.Center);

            if (Player.DistanceSQ(Projectile.Center) <= 270 * 270)
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
            /*for (int i = 0; i < Main.rand.Next(30, 60); i++)
            {
                Vector2 random = Utils.NextVector2Circular(Main.rand, 4, 4);
                if (Player.whoAmI == Main.myPlayer)
                {
                    Projectile shrapnel = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, random, ModContent.ProjectileType<Shrapnel>(), (int)(Projectile.damage * 0.1f), 1f);
                    shrapnel.GetGlobalProjectile<ProjPerkTracking>().ShotFromInsurgencyWeapon = true;
                    shrapnel.ai[0] = (float)PerkSystem.Perks.Demolitons;
                }
            }*/
        }
    }
}