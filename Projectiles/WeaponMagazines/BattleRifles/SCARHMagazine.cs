namespace InsurgencyWeapons.Projectiles.WeaponMagazines.BattleRifles
{
    internal class SCARHMagazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}