/*using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Shotguns;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Casings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.Shotguns
{
    /internal class IthacaHeld : WeaponBase
    {
        public int CurrentAmmo
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

        private bool AllowedToFire => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

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
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 82;
            MaxAmmo = 6;
            AmmoType = ModContent.ItemType<TwelveGauge>();
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
            CurrentAmmo = MagazineTracking.IthacaTube;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            Ammo ??= Player.FindItemInInventory(AmmoType);
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 4f;
            SpecificWeaponFix = new Vector2(0, 4f);

            if (AllowedToFire && !UnderAlternateFireCoolDown && PumpActionTimer == 0)
            {
                CurrentAmmo--;
                if (CurrentAmmo != 0)
                    PumpActionTimer = HeldItem.useTime * 2 - 40;
                ShotDelay = 0;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                for (int j = 0; j < 8; j++)
                {
                    Vector2 aim = Player.MountedCenter.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(10))) * HeldItem.shootSpeed;
                    //Buck
                    Shoot(aim, NormalBullet, BulletDamage, dropCasing: false);
                }
            }

            if (CurrentAmmo == 0 && Player.CountItem(AmmoType) > 0 && !ReloadStarted && PumpActionTimer == 0)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles;
                ReloadTimer = (int)(ReloadTimer * 0.75f);
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            switch (ReloadTimer)
            {
                case 15:
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = false;
                    break;

                case 40:
                    if (CurrentAmmo < MaxAmmo)
                    {
                        if (Ammo.stack > 0)
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                            Ammo.stack -= AmmoStackCount;
                            CurrentAmmo += AmmoStackCount;
                            ReloadTimer = 80;
                        }
                    }
                    break;

                case 120:
                    SoundEngine.PlaySound(PumpForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 160:
                    SoundEngine.PlaySound(Insert, Projectile.Center);
                    if (CurrentAmmo < MaxAmmo)
                    {
                        if (Ammo.stack > 0)
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                            Ammo.stack -= AmmoStackCount;
                            CurrentAmmo += AmmoStackCount;
                        }
                    }
                    break;

                case 200:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    SoundEngine.PlaySound(PumpBack, Projectile.Center);
                    DropCasingManually(ModContent.ProjectileType<Shells>(), frame: 1);
                    break;
            }

            switch (PumpActionTimer)
            {
                case 9:
                    SoundEngine.PlaySound(PumpForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 36:
                    SoundEngine.PlaySound(PumpBack, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    DropCasingManually(ModContent.ProjectileType<Shells>(), frame: 1);
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
}*/