namespace InsurgencyWeapons.Projectiles.Grenades
{
    internal class MK2Explosive : GrenadeBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            FuseTime = 4;
            base.SetDefaults();
        }
    }
}