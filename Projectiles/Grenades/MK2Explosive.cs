namespace InsurgencyWeapons.Projectiles.Grenades
{
    internal class MK2Explosive : GrenadeBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            FuseTime = 60 * 4f;
            Projectile.timeLeft = (int)FuseTime;
            base.SetDefaults();
        }
    }
}