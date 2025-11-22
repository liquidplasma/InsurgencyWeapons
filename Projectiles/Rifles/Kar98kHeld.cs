using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Rifles;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Rifles
{
    internal class Kar98kHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.Kar98kClip;
            }
            set
            {
                MagazineTracking.Kar98kClip = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/kar98/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private bool SemiAuto;

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/kar98/empty");
        private SoundStyle BoltRelease => new("InsurgencyWeapons/Sounds/Weapons/Ins2/kar98/bltrel");

        private SoundStyle BoltForward => new("InsurgencyWeapons/Sounds/Weapons/Ins2/kar98/bltfd", 2)
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
        };

        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/kar98/bltbk")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
        };

        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/kar98/ins")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
        };

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 26;
            MagazineSize = 5;
            BigSpriteSpecificIdlePos = true;
            AmmoType = ModContent.ItemType<Bullet79257>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1.33f, Projectile.height * 3 - 8);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.Kar98kClip;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 16f;
            SpecificWeaponFix = new Vector2(0, -1);
            if (!Player.channel)
                SemiAuto = false;

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && !SemiAuto && BoltActionTimer == 0)
            {
                SemiAuto = true;
                ShotDelay = 0;
                CurrentAmmo--;
                if (CurrentAmmo > 0)
                    BoltActionTimer = (int)(HeldItem.useTime * 1.75f);
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(1, dropCasing: false);
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted && BoltActionTimer == 0)
            {
                ReloadStarted = true;
                ReloadTimer = 13;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted && BoltActionTimer == 0)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.SniperRifles;
                ReloadStarted = true;
            }

            if (ReloadTimer > 0)
            {
                Player.SetDummyItemTime(2);
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CurrentAmmo != 0 && CurrentAmmo != MagazineSize && BoltActionTimer == 0)
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.SniperRifles;
                if (LiteMode)
                    ReloadTimer = 13;
            }

            switch (ReloadTimer)
            {
                case 1:
                    if (LiteMode)
                    {
                        if (CurrentAmmo < MagazineSize && CanReload())
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            ReloadSingle(13);
                        }
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 5:
                    if (!LiteMode)
                    {
                        SoundEngine.PlaySound(BoltForward, Projectile.Center);
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    }
                    break;

                case 10:
                    if (!LiteMode)
                        SoundEngine.PlaySound(BoltRelease, Projectile.Center);
                    break;

                case 20:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    if (CurrentAmmo < MagazineSize)
                    {
                        if (CanReload())
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            ammoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                            Player.ConsumeMultiple(ammoStackCount, Ammo.type);
                            CurrentAmmo += ammoStackCount;
                            ReloadTimer = 45;
                        }
                    }
                    break;

                case 90:
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    DropCasingManually();
                    if (ManualReload)
                    {
                        CurrentAmmo--;
                        Ammo.stack++;
                    }
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    break;
            }

            switch (BoltActionTimer)
            {
                case 10:
                    SoundEngine.PlaySound(BoltForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 20:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    break;

                case 39:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    break;

                case 42:
                    Projectile.frame = (int)Insurgency.MagazineState.Fired;
                    DropCasingManually();
                    break;

                case 45:
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<Kar98k>())
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