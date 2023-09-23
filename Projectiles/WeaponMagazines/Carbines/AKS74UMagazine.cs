namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Carbines
{
    internal class AKS74UMagazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}