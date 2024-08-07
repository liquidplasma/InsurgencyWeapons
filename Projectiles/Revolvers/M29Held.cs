﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Revolvers;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Revolvers
{
    public class M29Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M29Cylinder;
            }
            set
            {
                MagazineTracking.M29Cylinder = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Open => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/open");
        private SoundStyle Close => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/close");
        private SoundStyle Cock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/cock");
        private SoundStyle Dump => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/dump");
        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/ins");
        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/empty");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 36;
            MagazineSize = 6;
            AmmoType = ModContent.ItemType<Bullet44>();
            isPistol = true;
            drawScale = 1f;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height + 4);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M29Cylinder;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 9f;
            SpecificWeaponFix = new Vector2(-2 * Player.direction, -2f);

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && BoltActionTimer == 0)
            {
                BoltActionTimer = 60;
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(0, dropCasing: false);
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted && BoltActionTimer == 0)
            {
                ReloadStarted = true;
                ReloadTimer = 14;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted && BoltActionTimer == 0)
            {
                ReloadTimer = 180;
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0 && BoltActionTimer == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CanManualReload(CurrentAmmo))
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = 180;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(Close, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine(true);
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 35:
                    SoundEngine.PlaySound(Close, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 70:
                    if (CurrentAmmo < MagazineSize && CanReload())
                    {
                        if (ManualReload)
                            DropCasingManually();
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        ammoStackCount = Math.Clamp(Player.CountItem(AmmoType), 0, 1);
                        Player.ConsumeMultiple(ammoStackCount, Ammo.type);
                        CurrentAmmo += ammoStackCount;
                        ReloadTimer = 100;
                    }
                    break;

                case 125:
                    if (!ManualReload)
                    {
                        SoundEngine.PlaySound(Dump, Projectile.Center);
                        for (int i = 0; i < 6; i++)
                        {
                            DropCasingManually();
                        }
                    }
                    break;

                case 145:
                    SoundEngine.PlaySound(Open, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    break;
            }

            switch (BoltActionTimer)
            {
                case 5:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    break;

                case 10:
                    Projectile.frame = (int)Insurgency.MagazineState.Fired;
                    break;

                case 15:
                    SoundEngine.PlaySound(Cock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<M29>())
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