using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Projectiles.Grenades
{
    public class Shrapnel : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            base.SetDefaults();
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DisableCrit();
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void AI()
        {
            Timer++;
            if (Timer > 60)
            {
                if (Main.rand.NextBool(40))
                    Dust.NewDust(Projectile.Center, 2, 2, HelperStats.SmokeyDust);
                base.AI();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Bounce(15);
            return false;
        }
    }
}