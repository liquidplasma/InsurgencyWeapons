namespace InsurgencyWeapons.Projectiles.WeaponMagazines.AssaultRifles
{
    internal class ASVALMagazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}