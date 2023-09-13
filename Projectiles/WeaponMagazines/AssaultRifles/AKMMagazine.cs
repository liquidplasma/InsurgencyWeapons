namespace InsurgencyWeapons.Projectiles.WeaponMagazines.AssaultRifles
{
    internal class AKMMagazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 14;
            Projectile.penetrate = 5;
        }
    }
}