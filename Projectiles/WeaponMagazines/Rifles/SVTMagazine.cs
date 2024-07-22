namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Rifles
{
    public class SVTMagazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = 3;
            Projectile.height = 2;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}