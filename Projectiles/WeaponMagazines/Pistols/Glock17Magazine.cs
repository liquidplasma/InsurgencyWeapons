namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Pistols
{
    public class Glock17Magazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.scale = 0.67f;
            base.SetDefaults();
        }
    }
}