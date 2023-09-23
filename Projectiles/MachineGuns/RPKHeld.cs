using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.MachineGuns;
using InsurgencyWeapons.Projectiles.WeaponMagazines.MachineGuns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.MachineGuns
{
    internal class RPKHeld : WeaponBase
    {
        public int CurrentAmmo
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

        private bool AllowedToFire => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

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
            MaxAmmo = 75;
            AmmoType = ModContent.ItemType<Bullet762>();
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
            CurrentAmmo = MagazineTracking.RPKDrum;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            Ammo = Player.FindItemInInventory(AmmoType);
            Ammo ??= ContentSamples.ItemsByType[AmmoType];
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 18f;
            SpecificWeaponFix = new Vector2(0, -0.5f);
            if (AllowedToFire)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Vector2 aim = Player.MountedCenter.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(4))) * HeldItem.shootSpeed;

                Shoot(aim, BulletType, BulletDamage);
            }

            if (CurrentAmmo == 0 && Player.CountItem(Ammo.type) > 0 && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.LightMachineGuns;
                ReloadStarted = true;
            }

            if (Player.channel && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            switch (ReloadTimer)
            {
                case 30:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = false;
                    break;

                case 50:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;

                    if (Ammo.stack > 0)
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, MaxAmmo);
                        Ammo.stack -= AmmoStackCount;
                        CurrentAmmo = AmmoStackCount;
                    }
                    break;

                case 150:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    if (Player.whoAmI == Main.myPlayer)
                        DropMagazine(ModContent.ProjectileType<RPKDrum>());
                    break;

                case 180:
                    SoundEngine.PlaySound(MagRel, Projectile.Center);
                    break;
            }

            if (CurrentAmmo > 0 && Player.channel)
            {
                Projectile.frame = Math.Clamp(ShotDelay, 0, 2);
            }

            if (HeldItem.type != ModContent.ItemType<RPK>())
                Projectile.Kill();

            base.AI();
        }
    }
}