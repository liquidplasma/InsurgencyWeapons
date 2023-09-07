namespace InsurgencyWeapons.Projectiles.Magazines
{
    internal class STGMagazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.penetrate = 5;
        }
    }
}