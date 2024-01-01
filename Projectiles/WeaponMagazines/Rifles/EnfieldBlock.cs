namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Rifles
{
    internal class EnfieldBlock : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 6;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}