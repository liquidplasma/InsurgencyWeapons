using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.MachineGuns;
using InsurgencyWeapons.Projectiles.WeaponMagazines.MachineGuns;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.MachineGuns
{
    public class M249Held : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M249Box;
            }
            set
            {
                MagazineTracking.M249Box = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private bool CanManualReload() => CurrentAmmo != 0 && CurrentAmmo != MagazineSize;
        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/magin");
        private SoundStyle Throw => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/throw");
        private SoundStyle MagEMP => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/magemp");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/bltrel");
        private SoundStyle BoltRetract => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m60/brtrct");
        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/bltbk");
        private SoundStyle Open => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/open");
        private SoundStyle Close => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/close");
        private SoundStyle Hit => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m249/hit");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 84;
            MagazineSize = 200;
            AmmoType = ModContent.ItemType<Bullet556>();
            BigSpriteSpecificIdlePos = true;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            float scale;
            if (isIdle)
                scale = 0.75f;
            else
                scale = 0.8f;
            ExtensionMethods.BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.LightYellow, 56f, 1f, new Vector2(0, -3f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M249Box;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 12f;
            SpecificWeaponFix = new Vector2(0, -8f);
            if (AllowedToFire(CurrentAmmo))
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(3);
            }

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 370;
                ReloadStarted = true;
            }

            if (Player.channel && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CanManualReload())
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = 290;
            }

            switch (ReloadTimer)
            {
                case 30:
                    if (!ManualReload)
                        SoundEngine.PlaySound(Hit, Projectile.Center);
                    ReloadStarted = ManualReload = false;
                    break;

                case 100:
                    SoundEngine.PlaySound(Close, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    if (ManualReload)
                        ReloadTimer = 31;

                    break;

                case 120:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    if (CanReload())
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, MagazineSize);
                        Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                        CurrentAmmo = AmmoStackCount;
                    }
                    Projectile.frame = 1;
                    break;

                case 160:
                    SoundEngine.PlaySound(Throw, Projectile.Center);
                    if (ManualReload)
                    {
                        AmmoStackCount = CurrentAmmo;
                        Ammo.stack += AmmoStackCount;
                        CurrentAmmo = 0;
                    }
                    else
                        DropMagazine(ModContent.ProjectileType<M249Box>());

                    Projectile.frame = 2;
                    break;

                case 280:
                    SoundEngine.PlaySound(Open, Projectile.Center);
                    Projectile.frame = 1;
                    break;

                case 300:
                    SoundEngine.PlaySound(MagEMP, Projectile.Center);
                    break;

                case 320:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    break;

                case 339:
                    SoundEngine.PlaySound(BoltRetract, Projectile.Center);
                    break;

                case 360:
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<M249>())
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