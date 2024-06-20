using InsurgencyWeapons.Gores.Casing;
using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Shotguns;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Shotguns
{
    public class IthacaHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.IthacaTube;
            }
            set
            {
                MagazineTracking.IthacaTube = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/ithaca/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle PumpBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/ithaca/pmpbk")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle PumpForward => new("InsurgencyWeapons/Sounds/Weapons/Ins2/ithaca/pmpfd")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/ithaca/ins")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/ithaca/empty");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 84;
            MagazineSize = 6;
            drawScale = 0.8f;
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.IthacaTube;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 18f;
            SpecificWeaponFix = new Vector2(0, 3.5f);

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && PumpActionTimer == 0)
            {
                CurrentAmmo--;
                if (CurrentAmmo != 0 || LiteMode)
                    PumpActionTimer = 50;
                ShotDelay = 0;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                bool slug = AmmoType == ModContent.ItemType<TwelveGaugeSlug>();
                if (slug)
                    Shoot(0, false, slug: slug);
                else
                {
                    for (int j = 0; j < 8; j++)
                    {
                        //Buck
                        Shoot(1, dropCasing: false, shotgun: true);
                    }
                }
            }
            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted && PumpActionTimer == 0)
            {
                ReloadStarted = true;
                ReloadTimer = 13;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted && PumpActionTimer == 0)
            {
                ReloadTimer = 250;
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CurrentAmmo != 0 && CurrentAmmo != MagazineSize)
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = 70;
                if (LiteMode)
                    ReloadTimer = 13;
            }

            switch (ReloadTimer)
            {
                case 1:
                    if (LiteMode && CanReload() && CurrentAmmo < MagazineSize)
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        ReloadShotgun(13);
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 15:
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 40:
                    if (CurrentAmmo < MagazineSize && CanReload())
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        ReloadShotgun(70);
                    }

                    break;

                case 120:
                    if (!ManualReload)
                        SoundEngine.PlaySound(PumpForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 160:
                    if (!ManualReload && CurrentAmmo < MagazineSize)
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        if (Ammo.stack > 0)
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            ammoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                            Ammo.stack -= ammoStackCount;
                            CurrentAmmo += ammoStackCount;
                        }
                    }
                    break;

                case 200:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    SoundEngine.PlaySound(PumpBack, Projectile.Center);
                    if (AmmoType == ModContent.ItemType<TwelveGauge>())
                        DropCasingManually(ModContent.GoreType<ShellBuckShotGore>());
                    else
                        DropCasingManually(ModContent.GoreType<ShellSlugGore>());
                    break;
            }

            switch (PumpActionTimer)
            {
                case 4:
                    SoundEngine.PlaySound(PumpForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 14:
                    Projectile.frame = (int)Insurgency.MagazineState.Fired;
                    break;

                case 28:
                    SoundEngine.PlaySound(PumpBack, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (AmmoType == ModContent.ItemType<TwelveGauge>())
                        DropCasingManually(ModContent.GoreType<ShellBuckShotGore>());
                    else
                        DropCasingManually(ModContent.GoreType<ShellSlugGore>());
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<Ithaca>())
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