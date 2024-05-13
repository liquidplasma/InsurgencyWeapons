using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.AssaultRifles;
using InsurgencyWeapons.Projectiles.WeaponExtras;
using InsurgencyWeapons.Projectiles.WeaponMagazines.AssaultRifles;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.AssaultRifles
{
    public class AKMHeld : WeaponBase
    {
        public override int CurrentAmmo
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
            MagazineSize = 30;
            AmmoType = ModContent.ItemType<Bullet762>();
            GrenadeLauncherAmmoType = ModContent.ItemType<VOG_25P>();
            HasUnderBarrelGrenadeLauncer = true;
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.AKMMagazine;
            ShotDelay = HeldItem.useTime;
            PercentageOfAltFireCoolDown = 300 * 0.85f;
        }

        public override void AI()
        {
            if (Player.HasItem(GrenadeLauncherAmmoType))
                ShowAmmoCounter(CurrentAmmo, AmmoType, true, " VOG-25P: ", GrenadeLauncherAmmoType);
            else
                ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 12f;
            SpecificWeaponFix = new Vector2(0, 2f);
            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(5);
            }
            if (MouseRightPressed && Player.CountItem(GrenadeLauncherAmmoType) > 0 && AlternateFireCoolDown == 0 && !(ReloadTimer > 0))
            {
                AlternateFireCoolDown = 300;
                AmmoGL.stack--;
                SoundEngine.PlaySound(ShootGrenade, Projectile.Center);
                Vector2 aim = Player.MountedCenter.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(4))) * HeldItem.shootSpeed;
                VOGDamage = (int)(BulletDamage * 10f);

                float knockBack = Player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(HeldItem.knockBack);

                //VOG-25
                ExtensionMethods.BetterNewProjectile(
                    Player,
                    spawnSource: Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo),
                    position: Player.MountedCenter,
                    velocity: aim,
                    type: ModContent.ProjectileType<AKMVOG_25P>(),
                    damage: VOGDamage,
                    knockback: knockBack * 1.5f,
                    owner: Player.whoAmI);
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadStarted = true;
                ReloadTimer = 14;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.AssaultRifles;
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
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.AssaultRifles;
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
                    break;

                case 70:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    if (CanReload())
                        ReloadMagazine();
                    break;

                case 110:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    ReturnAmmo();
                    CurrentAmmo = 0;
                    if (!ManualReload)
                        DropMagazine(ModContent.ProjectileType<AKMMagazine>());
                    break;
            }

            if (CurrentAmmo > 0 && Player.channel)
                Projectile.frame = Math.Clamp(ShotDelay, 0, 2);

            if (HeldItem.type != ModContent.ItemType<AKM>())
                Projectile.Kill();

            base.AI();
        }

        private void ReloadRifle()
        {
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