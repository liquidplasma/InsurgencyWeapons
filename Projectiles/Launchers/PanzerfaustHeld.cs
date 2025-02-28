﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Launchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.DiscardedLaunchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.Warheads;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Launchers
{
    public class PanzerfaustHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get => MagazineTracking.Panzerfaust; set
            {
                MagazineTracking.Panzerfaust = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/faust/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Pin => new("InsurgencyWeapons/Sounds/Weapons/Ins2/faust/pin");
        private SoundStyle Sight => new("InsurgencyWeapons/Sounds/Weapons/Ins2/faust/sight");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 108;
            MagazineSize = 1;
            AmmoType = ModContent.ItemType<PZFaustRocket>();
            BigSpriteSpecificIdlePos = true;
            drawScale = 0.75f;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height);
            if (HelperStats.TestRange(ReloadTimer, 60, 120))
                return false;
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.Panzerfaust;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = -6;
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
                    ShootRocket(ModContent.ProjectileType<PanzerfaustWarhead>(), 1f);
                    break;
            }

            if (!Player.HasItem(AmmoType) && CurrentAmmo == 0)
                Projectile.frame = 1;

            if (LauncherDelay == 0 && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 180;
                Projectile.frame = 1;
                ReloadStarted = true;
            }
            switch (ReloadTimer)
            {
                case 1:
                    if (CanReload())
                        ReloadMagazine();
                    ReloadStarted = false;
                    SoundEngine.PlaySound(Sight, Projectile.Center);
                    break;

                case 60:
                    SoundEngine.PlaySound(Pin, Projectile.Center);
                    Projectile.frame = 0;
                    break;

                case 120:
                    DropMagazine(ModContent.ProjectileType<PanzerfaustDiscard>());
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<Panzerfaust>())
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