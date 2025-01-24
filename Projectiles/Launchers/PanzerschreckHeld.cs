using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Launchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.Warheads;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Launchers
{
    public class PanzerschreckHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get => MagazineTracking.Panzerschreck; set
            {
                MagazineTracking.Panzerschreck = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/schrck/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Load1 => new("InsurgencyWeapons/Sounds/Weapons/Ins2/schrck/load1");
        private SoundStyle Load2 => new("InsurgencyWeapons/Sounds/Weapons/Ins2/schrck/load2");
        private SoundStyle Wire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/schrck/wire");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 108;
            MagazineSize = 1;
            AmmoType = ModContent.ItemType<PanzerschreckRocket>();
            BigSpriteSpecificIdlePos = true;
            drawScale = 0.75f;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.Panzerschreck;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = -24;
            SpecificWeaponFix = new Vector2(0, -6);
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
                    ShootRocket(ModContent.ProjectileType<PanzerschreckWarhead>(), 0.8f);
                    break;
            }

            if (LauncherDelay == 0 && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 300;
                ReloadStarted = true;
            }
            switch (ReloadTimer)
            {
                case 1:
                    ReloadStarted = false;
                    Projectile.frame = 0;
                    break;

                case 90:
                    SoundEngine.PlaySound(Wire, Projectile.Center);
                    Projectile.frame = 1;
                    break;

                case 120:
                    SoundEngine.PlaySound(Load2, Projectile.Center);
                    Projectile.frame = 2;
                    if (CanReload())
                        ReloadMagazine();
                    break;

                case 180:
                    SoundEngine.PlaySound(Load1, Projectile.Center);
                    Projectile.frame = 3;
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<Panzerschreck>())
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