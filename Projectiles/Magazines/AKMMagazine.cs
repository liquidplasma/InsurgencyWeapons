namespace InsurgencyWeapons.Projectiles.Magazines
{
    internal class AKMMagazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.penetrate = 5;
        }
    }
}