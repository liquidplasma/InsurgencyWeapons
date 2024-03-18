namespace InsurgencyWeapons.Projectiles.WeaponMagazines.SubMachineGuns
{
    public class M1928Drum : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}