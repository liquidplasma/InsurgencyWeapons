using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.AssaultRifles;
using InsurgencyWeapons.Projectiles.WeaponExtras;
using InsurgencyWeapons.Projectiles.WeaponMagazines.AssaultRifles;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.AssaultRifles
{
    public class GrozaHeld : WeaponBase
    {
        private int CurrentAmmo
        {
            get
            {
                return MagazineTracking.GrozaMagazine;
            }
            set
            {
                MagazineTracking.GrozaMagazine = value;
            }
        }

        public int VOGDamage = 0;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/groza/shoot")
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

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/groza/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/groza/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/groza/magout");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/groza/bltfd");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 68;
            MagazineSize = 20;
            AmmoType = ModContent.ItemType<Bullet939>();
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
            CurrentAmmo = MagazineTracking.GrozaMagazine;
            ShotDelay = HeldItem.useTime;
            PercentageOfAltFireCoolDown = 300 * 0.85f;
        }

        public override void AI()
        {
            if (Player.HasItem(GrenadeLauncherAmmoType))
                ShowAmmoCounter(CurrentAmmo, AmmoType, true, " VOG-25P: ", GrenadeLauncherAmmoType);
            else
                ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 6f;
            SpecificWeaponFix = new Vector2(0, 0);
            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(3);
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

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
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
            }

            switch (ReloadTimer)
            {
                case 30:
                    if (!ManualReload)
                        SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = ManualReload = false;
                    break;

                case 70:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;

                    if (CanReload())
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, MagazineSize);
                        if (ManualReload)
                        {
                            AmmoStackCount++;
                            Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                            CurrentAmmo = AmmoStackCount;
                        }
                        else
                        {
                            Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                            CurrentAmmo = AmmoStackCount;
                        }
                    }
                    break;

                case 110:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    AmmoStackCount = CurrentAmmo;
                    Ammo.stack += AmmoStackCount;
                    CurrentAmmo = 0;
                    if (!ManualReload)
                    {
                        DropMagazine(ModContent.ProjectileType<GrozaMagazine>());
                    }
                    break;
            }

            if (CurrentAmmo > 0 && Player.channel)
            {
                Projectile.frame = Math.Clamp(ShotDelay, 0, 2);
            }

            if (HeldItem.type != ModContent.ItemType<Groza>())
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