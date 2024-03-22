using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Rifles;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Rifles;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Rifles
{
    public class M1GarandHeld : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.GarandMagazine;
            }
            set
            {
                MagazineTracking.GarandMagazine = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/garand/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle GarandPing => new("InsurgencyWeapons/Sounds/Weapons/Ins2/garand/ping", 5)
        {
            MaxInstances = 0,
            Volume = 0.25f
        };

        private bool SemiAuto;

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/garand/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/garand/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/garand/magout");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/garand/bltrel");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 68;
            MagazineSize = 8;
            AmmoType = ModContent.ItemType<Bullet3006>();
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
            CurrentAmmo = MagazineTracking.GarandMagazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 9f;
            SpecificWeaponFix = new Vector2(0, 0);

            if (!Player.channel || AutoAttack == 0)
            {
                SemiAuto = false;
                AutoAttack = HeldItem.useTime * 2;
            }

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && !SemiAuto)
            {
                SemiAuto = true;
                ShotDelay = 0;
                PumpActionTimer = HeldItem.useTime;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(2);
            }

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles;
                ReloadTimer += 100;
                SoundEngine.PlaySound(GarandPing, Projectile.Center);
                DropMagazine(ModContent.ProjectileType<M1GarandEnbloc>());
                Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
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
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles;
                ReloadTimer += 100;
            }

            switch (ReloadTimer)
            {
                case 25:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = ManualReload = false;
                    break;

                case 80:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    if (CanReload())
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, MagazineSize);
                        Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                        CurrentAmmo = AmmoStackCount;
                    }
                    break;

                case 150:
                    if (ManualReload)
                    {
                        AmmoStackCount = CurrentAmmo;
                        Ammo.stack += AmmoStackCount;
                        CurrentAmmo = 0;
                        SoundEngine.PlaySound(MagOut, Projectile.Center);
                        DropMagazine(ModContent.ProjectileType<M1GarandEnbloc>());
                    }
                    break;
            }

            if (CurrentAmmo != 0 && ReloadTimer == 0)
            {
                if (ShotDelay <= 3)
                    Projectile.frame = ShotDelay;
                else
                    Projectile.frame = 0;
            }

            if (HeldItem.type != ModContent.ItemType<M1Garand>())
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