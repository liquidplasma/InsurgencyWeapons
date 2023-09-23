namespace InsurgencyWeapons.Projectiles.WeaponMagazines.MachineGuns
{
    internal class RPKDrum : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}