namespace InsurgencyWeapons.Projectiles.Magazines.Casings
{
    internal class RifleCasing : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.penetrate = 8;
            Projectile.timeLeft = 1350;
        }
    }
}