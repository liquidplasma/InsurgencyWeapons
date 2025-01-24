namespace InsurgencyWeapons.Projectiles.WeaponExtras.Warheads
{
    public class PanzerfaustWarhead : WarheadBase
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.scale = 0.8f;
            base.SetDefaults();
        }

        public override void AI()
        {
            Projectile.Animate(9);
            base.AI();
        }
    }
}