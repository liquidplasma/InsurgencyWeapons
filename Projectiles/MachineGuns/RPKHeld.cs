﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.MachineGuns;
using InsurgencyWeapons.Projectiles.WeaponMagazines.MachineGuns;
using System.IO;

namespace InsurgencyWeapons.Projectiles.MachineGuns
{
    public class RPKHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.RPKDrum;
            }
            set
            {
                MagazineTracking.RPKDrum = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpk/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpk/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpk/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpk/magout");
        private SoundStyle MagRel => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpk/magrel");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpk/bltrel");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 86;
            MagazineSize = 75;
            AmmoType = ModContent.ItemType<Bullet762>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Gold, 1.25f, Projectile.height - 30);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.RPKDrum;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 18f;
            //SpecificWeaponFix = new Vector2(0, -0.5f);
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
                        SoundEngine.PlaySound(BoltLock, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine();
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 30:
                    if (!ManualReload)
                        SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = ManualReload = false;
                    break;

                case 50:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    if (CanReload())
                        ReloadMagazine();
                    break;

                case 150:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    ReturnAmmo();
                    CurrentAmmo = 0;
                    if (!ManualReload)
                        DropMagazine(ModContent.ProjectileType<RPKDrum>());

                    break;

                case 180:
                    SoundEngine.PlaySound(MagRel, Projectile.Center);
                    break;
            }

            if (CurrentAmmo > 0 && Player.channel)
                Projectile.frame = Math.Clamp(ShotDelay, 0, 2);

            if (HeldItem.type != ModContent.ItemType<RPK>())
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