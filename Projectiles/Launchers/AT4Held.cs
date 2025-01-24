using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Launchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.DiscardedLaunchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.Warheads;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Launchers
{
    public class AT4Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get => MagazineTracking.AT4; set
            {
                MagazineTracking.AT4 = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/at4/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        /// <summary>
        /// 10 frames before Fire
        /// </summary>
        private SoundStyle Latch => new("InsurgencyWeapons/Sounds/Weapons/Ins2/at4/latch");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 108;
            MagazineSize = 1;
            AmmoType = ModContent.ItemType<AT4Rocket>();
            BigSpriteSpecificIdlePos = true;
            drawScale = 0.75f;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height);
            if (HelperStats.TestRange(ReloadTimer, 20, 120))
                return false;
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.AT4;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = -16;
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
                    ShootRocket(ModContent.ProjectileType<AT4Warhead>(), 1.5f);
                    break;
            }
            if (LauncherDelay == 0 && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 200;
                ReloadStarted = true;
            }
            switch (ReloadTimer)
            {
                case 1:
                    if (CanReload())
                        ReloadMagazine();
                    ReloadStarted = false;
                    break;

                case 20:
                    SoundEngine.PlaySound(Latch, Projectile.Center);
                    break;

                case 120:
                    DropMagazine(ModContent.ProjectileType<AT4Discard>());
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<AT4>())
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