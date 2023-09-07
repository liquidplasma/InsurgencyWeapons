namespace InsurgencyWeapons.Projectiles.Magazines
{
    internal class AKMMagazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 6;
            Projectile.penetrate = 5;
        }
    }
}