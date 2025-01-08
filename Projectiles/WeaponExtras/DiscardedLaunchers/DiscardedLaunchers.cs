using InsurgencyWeapons.Projectiles.WeaponMagazines;

namespace InsurgencyWeapons.Projectiles.WeaponExtras.DiscardedLaunchers
{
    public class M72Discard : MagazineBase
    {
        public override string Texture => "InsurgencyWeapons/Projectiles/Launchers/M72LAWHeld";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 5;
            Projectile.scale = 0.75f;
            base.SetDefaults();
        }
    }

    public class AT4Discard : MagazineBase
    {
        public override string Texture => "InsurgencyWeapons/Projectiles/Launchers/AT4Held";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 5;
            Projectile.scale = 0.75f;
            base.SetDefaults();
        }
    }

    public class PanzerfaustDiscard : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 5;
            Projectile.scale = 0.8f;
            base.SetDefaults();
        }
    }
}