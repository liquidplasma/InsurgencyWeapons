﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.MachineGuns;
using InsurgencyWeapons.Projectiles.WeaponMagazines.MachineGuns;
using System.IO;

namespace InsurgencyWeapons.Projectiles.MachineGuns
{
    public class PKMHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.PKMBox;
            }
            set
            {
                MagazineTracking.PKMBox = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/pkm/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private bool CanManualReload() => CurrentAmmo != 0 && CurrentAmmo != MagazineSize;

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/pkm/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/pkm/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/pkm/magout");
        private SoundStyle BoltRetract => new("InsurgencyWeapons/Sounds/Weapons/Ins2/pkm/brtrct");
        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/pkm/bltbk");
        private SoundStyle LidUp => new("InsurgencyWeapons/Sounds/Weapons/Ins2/pkm/lidup");
        private SoundStyle LidDown => new("InsurgencyWeapons/Sounds/Weapons/Ins2/pkm/liddn");
        private SoundStyle Belt => new("InsurgencyWeapons/Sounds/Weapons/Ins2/pkm/belt");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 78;
            MagazineSize = 100;
            drawScale = 0.9f;
            AmmoType = ModContent.ItemType<Bullet76254R>();
            BigSpriteSpecificIdlePos = true;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Gold, 1.25f, Projectile.height - 22);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.PKMBox;
            ShotDelay = HeldItem.useTime;
        }

        public override bool PreAI()
        {
            if (isIdle)
                drawScale = 0.75f;
            else
                drawScale = 0.9f;
            return base.PreAI();
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 15f;
            //SpecificWeaponFix = new Vector2(0, -6);
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
                ReloadTimer = 320;
                ReloadStarted = true;
            }

            if (Player.channel && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CanManualReload(CurrentAmmo))
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = 320;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(LidDown, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine(true);
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 30:
                    SoundEngine.PlaySound(LidDown, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 80:
                    SoundEngine.PlaySound(Belt, Projectile.Center);
                    Projectile.frame = 6;
                    break;

                case 140:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = 5;
                    if (CanReload())
                        ReloadMagazine(true);
                    break;

                case 200:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    Projectile.frame = 4;
                    ReturnAmmo();
                    CurrentAmmo = 0;
                    if (!ManualReload)
                        DropMagazine(ModContent.ProjectileType<PKMBox>());
                    break;

                case 230:
                    if (CurrentAmmo != 0)
                    {
                        SoundEngine.PlaySound(Belt, Projectile.Center);
                        Projectile.frame = 5;
                    }
                    break;

                case 260:
                    SoundEngine.PlaySound(LidUp, Projectile.Center);
                    Projectile.frame = 5;
                    if (CurrentAmmo != 0)
                        Projectile.frame = 6;

                    break;

                case 280:
                    SoundEngine.PlaySound(BoltRetract, Projectile.Center);
                    break;
            }

            if (CurrentAmmo > 0 && Player.channel)
                Projectile.frame = Math.Clamp(ShotDelay, 0, 2);

            if (HeldItem.type != ModContent.ItemType<PKM>())
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