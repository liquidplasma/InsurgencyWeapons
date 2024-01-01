using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.MachineGuns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.MachineGuns
{
    internal class M60Held : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M60Box;
            }
            set
            {
                MagazineTracking.M60Box = value;
            }
        }

        private bool AllowedToFire => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/magin");
        private SoundStyle Throw => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/throw");
        private SoundStyle MagEMP => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/magemp");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/bltrel");
        private SoundStyle BoltRetract => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/brtrct");
        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/bltbk");
        private SoundStyle Open => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/open");
        private SoundStyle Close => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/close");
        private SoundStyle Hit => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/hit");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 80;
            MaxAmmo = 100;
            AmmoType = ModContent.ItemType<Bullet76251>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            ExtensionMethods.BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, 0.9f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.LightYellow, 56f, 1f, new Vector2(0, -3f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M60Box;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            Ammo ??= Player.FindItemInInventory(AmmoType);
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 18f;
            SpecificWeaponFix = new Vector2(0, -3.5f);
            if (AllowedToFire)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(1, 5, NormalBullet, BulletDamage);
            }

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * 2 * (int)Insurgency.ReloadModifiers.LightMachineGuns;
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
                ReloadTimer = HeldItem.useTime * 2 * (int)Insurgency.ReloadModifiers.LightMachineGuns;
            }

            switch (ReloadTimer)
            {
                case 30:
                    SoundEngine.PlaySound(Hit, Projectile.Center);
                    ReloadStarted = ManualReload = false;
                    break;

                case 100:
                    SoundEngine.PlaySound(Close, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 120:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    break;

                case 160:
                    SoundEngine.PlaySound(Throw, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;

                    if (CanReload())
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, MaxAmmo);
                        if (ManualReload)
                        {
                            Ammo.stack -= AmmoStackCount;
                            CurrentAmmo = AmmoStackCount;
                        }
                        else
                        {
                            Ammo.stack -= AmmoStackCount;
                            CurrentAmmo = AmmoStackCount;
                        }
                    }
                    break;

                case 210:
                    if (ManualReload)
                    {
                        AmmoStackCount = CurrentAmmo;
                        Ammo.stack += AmmoStackCount;
                        CurrentAmmo = 0;
                    }
                    break;

                case 280:
                    SoundEngine.PlaySound(Open, Projectile.Center);
                    Projectile.frame = 6;
                    break;

                case 300:
                    SoundEngine.PlaySound(MagEMP, Projectile.Center);
                    break;

                case 320:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = 5;
                    break;

                case 336:
                    Projectile.frame = 5;
                    break;

                case 339:
                    SoundEngine.PlaySound(BoltRetract, Projectile.Center);
                    break;

                case 341:
                    Projectile.frame = 4;
                    break;

                case 354:
                    Projectile.frame = 3;
                    break;

                case 357:
                    Projectile.frame = 2;
                    break;

                case 360:
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    Projectile.frame = 1;
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<M60>())
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