using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using Terraria.Graphics;

namespace InsurgencyWeapons.Projectiles
{
    public abstract class BulletBase : ModProjectile
    {
        public VertexStrip _vertexStrip = new();

        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Bullet;
        public Player Player => Main.player[Projectile.owner];
        public PerkSystem PerkTracking => Player.GetModPlayer<PerkSystem>();
        public Item HeldItem => Player.HeldItem;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 480;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = 750;
            Projectile.alpha = 255;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ShaderStuff.FancyTracer(_vertexStrip, Projectile);
            return base.PreDraw(ref lightColor);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            float mult = 1f;
            PerkSystem PerkTracking = Player.GetModPlayer<PerkSystem>();
            InsurgencyCustomSetBonusModPlayer SetTracking = Player.GetModPlayer<InsurgencyCustomSetBonusModPlayer>();
            if (PerkTracking.CommandoWeapons(HeldItem) && PerkTracking.Level[(int)PerkSystem.Perks.Commando] > 0)
                mult += PerkTracking.GetDamageMultPerLevel((int)PerkSystem.Perks.Commando);

            if (PerkTracking.SupportSpecialistWeapons(HeldItem) && PerkTracking.Level[(int)PerkSystem.Perks.SupportSpecialist] > 0)
                mult += PerkTracking.GetDamageMultPerLevel((int)PerkSystem.Perks.SupportSpecialist);

            if (PerkTracking.DemolitionsWeapons(HeldItem) && PerkTracking.Level[(int)PerkSystem.Perks.Demolitons] > 0)
                mult += PerkTracking.GetDamageMultPerLevel((int)PerkSystem.Perks.Demolitons);

            if (PerkTracking.SharpshooterWeapons(HeldItem) && PerkTracking.Level[(int)PerkSystem.Perks.Sharpshooter] > 0)
                mult += PerkTracking.GetDamageMultPerLevel((int)PerkSystem.Perks.Sharpshooter);

            if ((Insurgency.Pistols.Contains(HeldItem.type) || Insurgency.Revolvers.Contains(HeldItem.type)) && SetTracking.revolverSet)
                mult *= 2;

            if ((Insurgency.Carbines.Contains(HeldItem.type) || Insurgency.AssaultRifles.Contains(HeldItem.type)) && SetTracking.swatHelmet)
                mult *= 1.33f;

            if (HeldItem.ModItem is WeaponUtils sharpShooter && SetTracking.sniperScope)
            {
                if (sharpShooter.WeaponPerk == ((int)PerkSystem.Perks.Sharpshooter))
                {
                    mult *= 1.4f;
                }
            }

            if (InsurgencyModConfig.Instance.DamageScaling)
                mult *= Insurgency.WeaponScaling();
            modifiers.FinalDamage *= mult;
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void AI()
        {
            Projectile.FaceForward();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            return base.OnTileCollide(oldVelocity);
        }
    }

    public class NormalBullet : BulletBase
    {
        public override string Texture => base.Texture;

        public override void SetDefaults()
        {
            base.SetDefaults();
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

    public class ShotgunPellet : BulletBase
    {
        private int countPierce;

        public override string Texture => base.Texture;

        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            float penBuff = PerkTracking.GetPenetrationBuffSupport();
            Projectile.maxPenetrate = Projectile.penetrate = (int)Math.Round(3 * penBuff);
            Projectile.netUpdate = true;
            base.OnSpawn(source);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            countPierce++;
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            float pierceDecrease = 1f - (countPierce * (0.2f - PerkTracking.GetPenetrationBuffSupport() / 10f));
            modifiers.FinalDamage *= pierceDecrease;
            base.ModifyHitNPC(target, ref modifiers);
        }
    }
}