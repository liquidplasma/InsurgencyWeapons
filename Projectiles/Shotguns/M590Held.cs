﻿using InsurgencyWeapons.Gores.Casing;
using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Shotguns;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Shotguns
{
    public class M590Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M590Tube;
            }
            set
            {
                MagazineTracking.M590Tube = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle PumpBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/pmpbk")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle PumpForward => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/pmpfd")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/ins")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/empty");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 78;
            MagazineSize = 8;
            drawScale = 0.8f;
            isShotgun = true;
            AmmoType = ModContent.ItemType<TwelveGauge>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1.5f, Projectile.height - 24);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M590Tube;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 12f;
            SpecificWeaponFix = new Vector2(0, 0);

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && PumpActionTimer == 0)
            {
                CurrentAmmo--;
                if (CurrentAmmo != 0)
                    PumpActionTimer = 50;
                ShotDelay = 0;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                bool slug = AmmoType == ModContent.ItemType<TwelveGaugeSlug>();
                if (slug)
                    Shoot(0, false);
                else
                {
                    for (int j = 0; j < 8; j++)
                    {
                        //Buck
                        Shoot(1, dropCasing: false);
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
                    if (LiteMode)
                    {
                        if (CurrentAmmo < MagazineSize && CanReload())
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            ReloadShotgun(13);
                        }
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
                    if (!ManualReload && CurrentAmmo < MagazineSize && CanReload())
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        ammoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                        Player.ConsumeMultiple(ammoStackCount, Ammo.type);
                        CurrentAmmo += ammoStackCount;
                    }
                    break;

                case 200:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
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

            if (HeldItem.type != ModContent.ItemType<M590>())
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