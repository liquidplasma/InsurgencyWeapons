using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.MachineGuns;
using System.IO;

namespace InsurgencyWeapons.Projectiles.MachineGuns
{
    public class M60Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M60Box;
            }
            set
            {
                MagazineTracking.M60Box = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private bool CanManualReload() => CurrentAmmo != 0 && CurrentAmmo != MagazineSize;

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/magin");
        private SoundStyle Throw => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/throw");
        private SoundStyle MagEMP => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/magemp");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/bltrel");
        private SoundStyle BoltRetract => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/brtrct");
        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/bltbk");
        private SoundStyle Open => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/open");
        private SoundStyle Close => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/close");
        private SoundStyle Hit => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/hit");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 80;
            MagazineSize = 100;
            drawScale = 0.9f;
            AmmoType = ModContent.ItemType<Bullet76251>();
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M60Box;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            if (isIdle)
                drawScale = 0.75f;
            else
                drawScale = 0.9f;
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 18f;
            SpecificWeaponFix = new Vector2(0, -3.5f);
            if (AllowedToFire(CurrentAmmo))
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(3);
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadStarted = true;
                ReloadTimer = 14;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.LightMachineGuns;
                ReloadStarted = true;
            }

            if (Player.channel && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CanManualReload(CurrentAmmo))
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.LightMachineGuns;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(Throw, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine(true);
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 30:
                    SoundEngine.PlaySound(Hit, Projectile.Center);
                    break;

                case 100:
                    SoundEngine.PlaySound(Close, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 120:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    break;

                case 160:
                    SoundEngine.PlaySound(Throw, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    if (CanReload())
                        ReloadMagazine(true);
                    break;

                case 210:
                    ReturnAmmo();
                    break;

                case 280:
                    SoundEngine.PlaySound(Open, Projectile.Center);
                    Projectile.frame = 6;
                    break;

                case 300:
                    SoundEngine.PlaySound(MagEMP, Projectile.Center);
                    break;

                case 320:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = 5;
                    break;

                case 336:
                    Projectile.frame = 5;
                    break;

                case 339:
                    SoundEngine.PlaySound(BoltRetract, Projectile.Center);
                    break;

                case 341:
                    Projectile.frame = 4;
                    break;

                case 354:
                    Projectile.frame = 3;
                    break;

                case 357:
                    Projectile.frame = 2;
                    break;

                case 360:
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    Projectile.frame = 1;
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<M60>())
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