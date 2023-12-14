using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles
{
    internal abstract class BulletBase : ModProjectile
    {
        public VertexStrip _vertexStrip = new();
        public int CaliberSize => (int)Projectile.ai[0];
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Bullet;
        public Player Player => Main.player[Projectile.owner];
        public Item HeldItem => Player.HeldItem;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 800;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = 900;
            Projectile.alpha = 255;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, ShaderStuff.WhiteTrail, ShaderStuff.NormalBulletStripWidth, -Main.screenPosition + Projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            Projectile.FaceForward();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            return base.OnTileCollide(oldVelocity);
        }
    }

    internal class NormalBullet : BulletBase
    {
        public override string Texture => base.Texture;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            switch (CaliberSize)
            {
                case (int)Insurgency.APCaliber.c762x51mm:
                    modifiers.ArmorPenetration += 12;
                    break;

                case (int)Insurgency.APCaliber.c303mm:
                    modifiers.ArmorPenetration += 25;
                    break;

                case (int)Insurgency.APCaliber.c762x63mm:
                    modifiers.ArmorPenetration += 25;
                    break;

                case (int)Insurgency.APCaliber.c762x54Rmm:
                    modifiers.ArmorPenetration += 35;
                    break;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Insurgency.SniperRifles.Contains(HeldItem.type))
            {
                Projectile.extraUpdates += 5;
                Projectile.netUpdate = true;
            }
            if (Insurgency.Rifles.Contains(HeldItem.type))
            {
                Projectile.extraUpdates += 3;
                Projectile.netUpdate = true;
            }
        }
    }

    internal class ShotgunPellet : BulletBase
    {
        private int countPierce;
        public override string Texture => base.Texture;

        public override void SetDefaults()
        {
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            base.SetDefaults();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            countPierce++;
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (countPierce > 0)
            {
                float pierceDecrease = 1f - (countPierce * 0.15f);
                modifiers.FinalDamage *= pierceDecrease;
            }
            base.ModifyHitNPC(target, ref modifiers);
        }
    }
}