namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Pistols
{
    public class M1911Magazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 4;
            base.SetDefaults();
        }
    }
}