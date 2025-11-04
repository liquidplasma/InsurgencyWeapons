namespace InsurgencyWeapons.Projectiles.WeaponExtras.Warheads
{
    internal class Grenade40mmProj : WarheadBase
    {
        public override void AI()
        {
            if (FlyTimeSeconds >= 2)
                Projectile.velocity.Y += 0.025f;
            base.AI();
        }
    }
}