using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Rifles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.Rifles
{
    internal class EnfieldHeld : WeaponBase
    {
        public int CurrentAmmo
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

        private bool AllowedToFire => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private bool SemiAuto;

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/magin");
        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/ins");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/bltrel");
        private SoundStyle BoltForward => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/bltfd");
        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/enfield/bltbk");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 80;
            MaxAmmo = 10;
            AmmoType = ModContent.ItemType<Bullet76251>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            ExtensionMethods.BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, 0.9f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.Yellow, 44f, 1f, new Vector2(0, -4f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.EnfieldMagazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            Ammo ??= Player.FindItemInInventory(AmmoType);
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 4f;
            SpecificWeaponFix = new Vector2(0, 0);

            if (AllowedToFire && !UnderAlternateFireCoolDown && BoltActionTimer == 0)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                BoltActionTimer = 100;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Vector2 aim = Player.MountedCenter.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(2))) * HeldItem.shootSpeed;
                Shoot(aim, NormalBullet, BulletDamage, dropCasing: false, ai0: (float)Insurgency.APCaliber.c762x51mm);
            }
            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles;
                ReloadTimer -= 180;
                Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            switch (ReloadTimer)
            {
                case 20:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = false;
                    break;

                case 54:
                    SoundEngine.PlaySound(BoltForward, Projectile.Center);
                    break;

                case 80:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    if (CurrentAmmo < MaxAmmo)
                    {
                        if (CanReload(5))
                        {
                            SoundEngine.PlaySound(MagIn, Projectile.Center);
                            AmmoStackCount = Math.Clamp(Player.CountItem(AmmoType), 0, 5);
                            Ammo.stack -= AmmoStackCount;
                            CurrentAmmo += AmmoStackCount;
                            ReloadTimer = 140;
                        }
                        else if (CanReload())
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            AmmoStackCount = Math.Clamp(Player.CountItem(AmmoType), 0, 1);
                            Ammo.stack -= AmmoStackCount;
                            CurrentAmmo += AmmoStackCount;
                            ReloadTimer = 120;
                        }
                    }
                    break;

                case 165:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    DropCasingManually();
                    break;
            }

            switch (BoltActionTimer)
            {
                case 40:
                    SoundEngine.PlaySound(BoltForward, Projectile.Center);
                    break;

                case 70:
                    if (CurrentAmmo != 0)
                    {
                        SoundEngine.PlaySound(BoltBack, Projectile.Center);
                        DropCasingManually();
                    }
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<LeeEnfield>())
                Projectile.Kill();

            base.AI();
        }
    }
}