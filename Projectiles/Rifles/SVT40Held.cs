using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Rifles;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Rifles
{
    internal class SVT40Held : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.SVTMagazine;
            }
            set
            {
                MagazineTracking.SVTMagazine = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/svt40/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/svt40/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/svt40/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/svt40/magout");
        private SoundStyle MagRel => new("InsurgencyWeapons/Sounds/Weapons/Ins2/svt40/magrel");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/svt40/bltrel");
        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/svt40/bltbk");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 82;
            ClipSize = 10;
            AmmoType = ModContent.ItemType<Bullet76254R>();
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
            OffsetFromPlayerCenter = 14f;
            SpecificWeaponFix = new Vector2(0, 0);

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(1, 2, ai0: (float)Insurgency.APCaliber.c762x54Rmm);
            }

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles;
                ReloadTimer += 30;
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
                ReloadTimer += 30;
            }

            switch (ReloadTimer)
            {
                case 15:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = ManualReload = false;
                    break;

                case 40:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (CanReload())
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, ClipSize);
                        Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                        CurrentAmmo = AmmoStackCount;
                    }
                    break;

                case 80:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    if (ManualReload)
                    {
                        AmmoStackCount = CurrentAmmo;
                        Ammo.stack += AmmoStackCount;
                        CurrentAmmo = 0;
                    }
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    break;

                case 120:
                    SoundEngine.PlaySound(MagRel, Projectile.Center);
                    break;

                case 130:
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    break;
            }
            if (CurrentAmmo != 0 && ReloadTimer == 0)
            {
                if (ShotDelay <= 3)
                    Projectile.frame = ShotDelay;
                else
                    Projectile.frame = 0;
            }

            if (HeldItem.type != ModContent.ItemType<SVT40>())
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