namespace InsurgencyWeapons.Projectiles.Grenades
{
    internal class M24StExplosive : GrenadeBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            FuseTime = 4;
            base.SetDefaults();
        }
    }
}