using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Launchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.DiscardedLaunchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.Warheads;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Launchers
{
    public class M72LAWHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get => MagazineTracking.M72Law; set
            {
                MagazineTracking.M72Law = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/law/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        /// <summary>
        /// 10 frames before Fire
        /// </summary>
        private SoundStyle Safe => new("InsurgencyWeapons/Sounds/Weapons/Ins2/law/safe");

        private SoundStyle Low => new("InsurgencyWeapons/Sounds/Weapons/Ins2/law/low");
        private SoundStyle Tap => new("InsurgencyWeapons/Sounds/Weapons/Ins2/law/tap");
        private SoundStyle Open => new("InsurgencyWeapons/Sounds/Weapons/Ins2/law/open");
        private SoundStyle Arm => new("InsurgencyWeapons/Sounds/Weapons/Ins2/law/arm");
        private SoundStyle Latch => new("InsurgencyWeapons/Sounds/Weapons/Ins2/law/latch");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 102;
            MagazineSize = 1;
            AmmoType = ModContent.ItemType<M72LAWRocket>();
            BigSpriteSpecificIdlePos = true;
            drawScale = 0.75f;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height);
            if (HelperStats.TestRange(ReloadTimer, 130, 160))
                return false;
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M72Law;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 6f;
            SpecificWeaponFix = new Vector2(0, -1);
            if (LauncherDelay == 0 && AllowedToFire(CurrentAmmo))
            {
                ShotDelay = 0;
                LauncherDelay = 11;
            }
            switch (LauncherDelay)
            {
                case 1:
                    CurrentAmmo--;
                    SoundEngine.PlaySound(Fire, Projectile.Center);
                    ShootRocket(ModContent.ProjectileType<M72LAWWarhead>(), 1.2f);
                    break;

                case 2:
                    SoundEngine.PlaySound(Safe, Projectile.Center);
                    break;
            }
            if (LauncherDelay == 0 && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 270;
                ReloadStarted = true;
            }
            switch (ReloadTimer)
            {
                case 1:
                    ReloadStarted = false;
                    break;

                case 10:
                    SoundEngine.PlaySound(Latch, Projectile.Center);
                    break;

                case 40:
                    SoundEngine.PlaySound(Arm, Projectile.Center);
                    break;

                case 70:
                    SoundEngine.PlaySound(Open, Projectile.Center);
                    break;

                case 100:
                    SoundEngine.PlaySound(Tap, Projectile.Center);
                    break;

                case 130:
                    SoundEngine.PlaySound(Low, Projectile.Center);
                    ReloadMagazine();
                    break;

                case 160:
                    DropMagazine(ModContent.ProjectileType<M72Discard>());
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<M72LAW>())
                Projectile.Kill();
            base.AI();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(CurrentAmmo);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            CurrentAmmo = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }
    }
}