using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.AssaultRifles;
using InsurgencyWeapons.Projectiles.WeaponExtras;
using InsurgencyWeapons.Projectiles.WeaponMagazines.AssaultRifles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.AssaultRifles
{
    internal class AKMHeld : WeaponBase
    {
        private int CurrentAmmo
        {
            get
            {
                return MagazineTracking.AKMMagazine;
            }
            set
            {
                MagazineTracking.AKMMagazine = value;
            }
        }

        public int VOGDamage = 0;
        private bool AllowedToFire => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/akm/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle ShootGrenade => new("InsurgencyWeapons/Sounds/Weapons/Ins2/akm/shootGrenade")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/akm/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/akm/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/akm/magout");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/akm/bltrel");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 74;
            MaxAmmo = 30;
            AmmoType = ModContent.ItemType<Bullet762>();
            GrenadeLauncherAmmoType = ModContent.ItemType<VOG_25P>();
            HasUnderBarrelGrenadeLauncer = true;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            ExtensionMethods.BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, 0.9f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.Yellow, 48f, 1f, new Vector2(0, -4.25f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.AKMMagazine;
            ShotDelay = HeldItem.useTime;
            PercentageOfAltFireCoolDown = 300 * 0.85f;
        }

        public override void AI()
        {
            Ammo = Player.FindItemInInventory(AmmoType);
            AmmoGL = Player.FindItemInInventory(GrenadeLauncherAmmoType);
            AmmoGL ??= ContentSamples.ItemsByType[GrenadeLauncherAmmoType];
            ShowAmmoCounter(CurrentAmmo, AmmoType, true, " VOG-25P: ", GrenadeLauncherAmmoType);
            OffsetFromPlayerCenter = 12f;
            if (AllowedToFire && !UnderAlternateFireCoolDown)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Vector2 aim = Player.MountedCenter.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(5))) * HeldItem.shootSpeed;
                Shoot(aim, NormalBullet, BulletDamage);
            }
            if (MouseRightPressed && Player.CountItem(GrenadeLauncherAmmoType) > 0 && AlternateFireCoolDown == 0 && !(ReloadTimer > 0))
            {
                AlternateFireCoolDown = 300;
                AmmoGL.stack--;
                SoundEngine.PlaySound(ShootGrenade, Projectile.Center);
                Vector2 aim = Player.MountedCenter.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(4))) * HeldItem.shootSpeed;
                VOGDamage = (int)((Projectile.damage + Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Ammo.damage)) * Player.GetStealth() * (5f * Insurgency.WeaponScaling()));

                //VOG-25
                ExtensionMethods.BetterNewProjectile(
                    Player,
                    spawnSource: Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo),
                    position: Player.MountedCenter,
                    velocity: aim,
                    type: ModContent.ProjectileType<AKMVOG_25P>(),
                    damage: VOGDamage,
                    knockback: 0f,
                    owner: Player.whoAmI);
            }

            if (CurrentAmmo == 0 && Player.CountItem(Ammo.type) > 0 && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.AssaultRifles;
                ReloadStarted = true;
            }
            if (Player.channel && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            switch (ReloadTimer)
            {
                case 15:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = false;
                    break;

                case 40:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;

                    if (CanReload())
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, MaxAmmo);
                        Ammo.stack -= AmmoStackCount;
                        CurrentAmmo = AmmoStackCount;
                    }
                    break;

                case 80:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    if (Player.whoAmI == Main.myPlayer)
                        DropMagazine(ModContent.ProjectileType<AKMMagazine>());
                    break;
            }

            if (CurrentAmmo > 0 && Player.channel)
            {
                Projectile.frame = Math.Clamp(ShotDelay, 0, 2);
            }

            if (HeldItem.type != ModContent.ItemType<AKM>())
                Projectile.Kill();

            base.AI();
        }
    }
}