using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Rifles;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Rifles;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Rifles
{
    public class EnfieldHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.EnfieldMagazine;
            }
            set
            {
                MagazineTracking.EnfieldMagazine = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/magin");
        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/ins");

        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/bltrel")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle BoltForward => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/bltfd")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/bltbk")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 80;
            MagazineSize = 10;
            AmmoType = ModContent.ItemType<Bullet303>();
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.EnfieldMagazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 6f;
            SpecificWeaponFix = new Vector2(0, 1f);

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && BoltActionTimer == 0)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                BoltActionTimer = 70;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(2, dropCasing: false);
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadStarted = true;
                ReloadTimer = 15;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 180;
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
                ReloadTimer = 180;
                if (LiteMode)
                    ReloadTimer = 15;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(BoltLock, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine(true);
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 20:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 54:
                    SoundEngine.PlaySound(BoltForward, Projectile.Center);
                    break;

                case 80:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (CurrentAmmo < MagazineSize)
                    {
                        if (CurrentAmmo <= 5 && CanReload(5))
                        {
                            SoundEngine.PlaySound(MagIn, Projectile.Center);
                            ammoStackCount = Math.Clamp(Player.CountItem(AmmoType), 0, 5);
                            Player.ConsumeMultiple(ammoStackCount, Ammo.type);
                            CurrentAmmo += ammoStackCount;
                            DropMagazine(ModContent.ProjectileType<EnfieldBlock>());
                            ReloadTimer = 140;
                        }
                        else if (CanReload())
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            ammoStackCount = Math.Clamp(Player.CountItem(AmmoType), 0, 1);
                            Player.ConsumeMultiple(ammoStackCount, Ammo.type);
                            CurrentAmmo += ammoStackCount;
                            ReloadTimer = 110;
                        }
                    }
                    break;

                case 165:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    DropCasingManually();
                    if (ManualReload)
                    {
                        CurrentAmmo--;
                        Ammo.stack++;
                    }
                    break;
            }

            switch (BoltActionTimer)
            {
                case 10:
                    SoundEngine.PlaySound(BoltForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Fired;
                    break;

                case 20:
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 30:
                    if (CurrentAmmo != 0)
                    {
                        SoundEngine.PlaySound(BoltBack, Projectile.Center);
                        Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                        DropCasingManually();
                    }
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<LeeEnfield>())
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