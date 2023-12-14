namespace InsurgencyWeapons.Projectiles.WeaponMagazines.AssaultRifles
{
    internal class AN94Magazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}