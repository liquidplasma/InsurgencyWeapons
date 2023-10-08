using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Shotguns;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Casings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.Shotguns
{
    internal class CoachHeld : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.CoachBarrel;
            }
            set
            {
                MagazineTracking.CoachBarrel = value;
            }
        }

        private enum CoachState
        {
            Ready,
            Open
        }

        private bool AllowedToFire => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/coach/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle FireBoth => new("InsurgencyWeapons/Sounds/Weapons/Ins2/coach/sboth")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private bool SemiAuto;

        private SoundStyle Open => new("InsurgencyWeapons/Sounds/Weapons/Ins2/coach/open");
        private SoundStyle Close => new("InsurgencyWeapons/Sounds/Weapons/Ins2/coach/close");

        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/coach/ins", 2);
        private SoundStyle Eject => new("InsurgencyWeapons/Sounds/Weapons/Ins2/coach/eject");
        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/coach/empty");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 72;
            MaxAmmo = 2;
            AmmoType = ModContent.ItemType<ShellBuck_Ball>();
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
            CurrentAmmo = MagazineTracking.CoachBarrel;
            ShotDelay = HeldItem.useTime;
            PercentageOfAltFireCoolDown = 180 * 0.85f;
        }

        public override void AI()
        {
            Ammo ??= Player.FindItemInInventory(AmmoType);
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 12f;
            SpecificWeaponFix = new Vector2(0, 4f);
            if (!Player.channel)
                SemiAuto = false;

            bool CanFireBothBarrels = MouseRightPressed && CanFire && CurrentAmmo == 2 && ReloadTimer == 0;
            if ((AllowedToFire || CanFireBothBarrels) && !UnderAlternateFireCoolDown && !SemiAuto)
            {
                SemiAuto = true;
                ShotDelay = 0;

                if (CanFireBothBarrels)
                    SoundEngine.PlaySound(FireBoth, Projectile.Center);
                else
                    SoundEngine.PlaySound(Fire, Projectile.Center);

                int both = 1;
                if (CanFireBothBarrels)
                {
                    AlternateFireCoolDown = 180;
                    both = 2;
                }

                for (int i = 0; i < both; i++)
                {
                    CurrentAmmo--;
                    for (int j = 0; j < 6; j++)
                    {
                        Vector2 aim = Player.MountedCenter.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(14))) * HeldItem.shootSpeed;
                        //Buck
                        Shoot(aim, NormalBullet, BulletDamage, dropCasing: false);
                    }

                    Vector2 aimNoSpread = Player.MountedCenter.DirectionTo(MouseAim) * HeldItem.shootSpeed;
                    //Ball
                    Shoot(aimNoSpread, NormalBullet, BulletDamage, dropCasing: false);
                }
            }

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.AssaultRifles;
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
                    SoundEngine.PlaySound(Close, Projectile.Center);
                    Projectile.frame = (int)CoachState.Ready;
                    ReloadStarted = false;
                    break;

                case 108:
                    if (CanReload())
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                        Ammo.stack -= AmmoStackCount;
                        CurrentAmmo += AmmoStackCount;
                    }
                    break;

                case 132:
                    if (CanReload())
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                        Ammo.stack -= AmmoStackCount;
                        CurrentAmmo += AmmoStackCount;
                    }
                    break;

                case 162:
                    SoundEngine.PlaySound(Eject, Projectile.Center);
                    for (int i = 0; i < 2; i++)
                    {
                        DropCasingManually(ModContent.ProjectileType<Shells>());
                    }
                    break;

                case 180:
                    Projectile.frame = (int)CoachState.Open;
                    SoundEngine.PlaySound(Open, Projectile.Center);
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<Coach>())
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