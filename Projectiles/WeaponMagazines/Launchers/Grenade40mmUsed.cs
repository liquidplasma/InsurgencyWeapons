namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Launchers
{
    internal class Grenade40mmUsed : MagazineBase
    {
        private SoundStyle Drop => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m79/drop");

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(Drop with
            {
                Volume = 0.08f,
                Pitch = Main.rand.NextFloat(-0.12f, 0.12f),
            }, Projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }
    }
}