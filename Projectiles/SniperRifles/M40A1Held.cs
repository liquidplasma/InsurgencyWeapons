using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.SniperRifles;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.SniperRifles
{
    public class M40A1Held : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M40A1Box;
            }
            set
            {
                MagazineTracking.M40A1Box = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m40a1/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private bool SemiAuto;

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m40a1/empty");
        private SoundStyle BoltRelease => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m40a1/bltrel");

        private SoundStyle BoltForward => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m40a1/bltfd")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
        };

        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m40a1/bltbk")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
        };

        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m40a1/ins")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
        };

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 82;
            MagazineSize = 5;
            AmmoType = ModContent.ItemType<Bullet76251>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, 0.9f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.Yellow, 44f, 1f, new Vector2(0, -4f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M40A1Box;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 4f;
            SpecificWeaponFix = new Vector2(0, -2f);
            if (!Player.channel)
                SemiAuto = false;

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && !SemiAuto && BoltActionTimer == 0)
            {
                SemiAuto = true;
                ShotDelay = 0;
                CurrentAmmo--;
                if (CurrentAmmo > 0)
                    BoltActionTimer = HeldItem.useTime * 2;

                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(1, dropCasing: false);
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted && BoltActionTimer == 0)
            {
                ReloadStarted = true;
                ReloadTimer = 13;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted && BoltActionTimer == 0)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.SniperRifles;
                ReloadStarted = true;
            }

            if (ReloadTimer > 0)
            {
                Player.SetDummyItemTime(2);
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CurrentAmmo != 0 && CurrentAmmo != MagazineSize && BoltActionTimer == 0)
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.SniperRifles;
                if (LiteMode)
                    ReloadTimer = 13;
            }

            switch (ReloadTimer)
            {
                case 1:
                    if (LiteMode)
                    {
                        if (CurrentAmmo < MagazineSize && CanReload())
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            CurrentAmmo = ReloadShotgun(CurrentAmmo, 13);
                        }
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 5:
                    if (!LiteMode)
                    {
                        SoundEngine.PlaySound(BoltForward, Projectile.Center);
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    }
                    break;

                case 10:
                    if (!LiteMode)
                        SoundEngine.PlaySound(BoltRelease, Projectile.Center);
                    break;

                case 20:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    if (CurrentAmmo < MagazineSize)
                    {
                        if (CanReload())
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                            Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                            CurrentAmmo += AmmoStackCount;
                            ReloadTimer = 50;
                        }
                    }
                    break;

                case 90:
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    DropCasingManually();
                    if (ManualReload)
                    {
                        CurrentAmmo--;
                        Ammo.stack++;
                    }
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    break;
            }

            switch (BoltActionTimer)
            {
                case 10:
                    SoundEngine.PlaySound(BoltForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 20:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    break;

                case 39:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    break;

                case 42:
                    Projectile.frame = (int)Insurgency.MagazineState.Fired;
                    DropCasingManually();
                    break;

                case 45:
                    SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<M40A1>())
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