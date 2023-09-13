namespace InsurgencyWeapons.Projectiles.WeaponMagazines.AssaultRifles
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