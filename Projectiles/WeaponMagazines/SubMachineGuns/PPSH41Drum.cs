namespace InsurgencyWeapons.Projectiles.WeaponMagazines.SubMachineGuns
{
    public class PPSH41Drum : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}