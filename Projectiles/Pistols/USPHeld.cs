﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Pistols;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Pistols;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Pistols
{
    public class USPHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.USPMagazine;
            }
            set
            {
                MagazineTracking.USPMagazine = value;
            }
        }

        private bool SemiAuto;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/usp/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/usp/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/usp/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/usp/magout");
        private SoundStyle SlideRel => new("InsurgencyWeapons/Sounds/Weapons/Ins2/usp/sldrel");
        private SoundStyle SlideBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/usp/sldbk");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 48;
            MagazineSize = 12;
            AmmoType = ModContent.ItemType<Bullet45ACP>();
            drawScale = 0.67f;
            isPistol = true;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height - 14);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.USPMagazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 6f;
            SpecificWeaponFix = new Vector2(0, -1);

            if (!Player.channel || AutoAttack == 0)
            {
                SemiAuto = false;
                AutoAttack = (int)(HeldItem.useTime * 1.5f);
            }

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && !SemiAuto)
            {
                SemiAuto = true;
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(2);
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadStarted = true;
                ReloadTimer = 14;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles;
                ReloadTimer += 90;
                SoundEngine.PlaySound(SlideBack, Projectile.Center);
                Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CanManualReload(CurrentAmmo))
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles;
                ReloadTimer += 90;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(SlideRel, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine();
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 15:
                    if (!ManualReload)
                        SoundEngine.PlaySound(SlideRel, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 60:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    if (!ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (CanReload())
                        ReloadMagazine();
                    break;

                case 120:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    ReturnAmmo();
                    CurrentAmmo = 0;
                    if (!ManualReload)
                    {
                        DropMagazine(ModContent.ProjectileType<USPMagazine>());
                        Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    }
                    break;
            }
            if (CurrentAmmo != 0 && ReloadTimer == 0)
            {
                if (ShotDelay <= 3)
                    Projectile.frame = ShotDelay;
                else
                    Projectile.frame = 0;
            }

            if (HeldItem.type != ModContent.ItemType<USP>())
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