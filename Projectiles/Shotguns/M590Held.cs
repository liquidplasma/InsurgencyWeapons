using InsurgencyWeapons.Gores.Casing;
using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Shotguns;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Shotguns
{
    public class M590Held : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M590Tube;
            }
            set
            {
                MagazineTracking.M590Tube = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle PumpBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/pmpbk")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle PumpForward => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/pmpfd")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/ins")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m590/empty");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 78;
            MagazineSize = 8;
            AmmoType = ModContent.ItemType<TwelveGauge>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            ExtensionMethods.BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, 0.8f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.Yellow, 44f, 1f, new Vector2(0, -4f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M590Tube;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 0f;
            SpecificWeaponFix = new Vector2(0, 2f);

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && PumpActionTimer == 0)
            {
                CurrentAmmo--;
                if (CurrentAmmo != 0)
                    PumpActionTimer = 50;
                ShotDelay = 0;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                for (int j = 0; j < 8; j++)
                {
                    //Buck
                    Shoot(1, dropCasing: false, shotgun: true);
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

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CurrentAmmo != 0 && CurrentAmmo != MagazineSize)
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = 120;
            }

            switch (ReloadTimer)
            {
                case 15:
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = ManualReload = false;
                    break;

                case 40:
                    if (CurrentAmmo < MagazineSize)
                    {
                        if (Ammo.stack > 0)
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                            Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                            CurrentAmmo += AmmoStackCount;
                            ReloadTimer = 70;
                        }
                    }
                    break;

                case 120:
                    if (!ManualReload)
                        SoundEngine.PlaySound(PumpForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 160:
                    if (!ManualReload && CurrentAmmo < MagazineSize)
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        if (Ammo.stack > 0)
                        {
                            SoundEngine.PlaySound(Insert, Projectile.Center);
                            AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                            Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                            CurrentAmmo += AmmoStackCount;
                        }
                    }
                    break;

                case 200:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    SoundEngine.PlaySound(PumpBack, Projectile.Center);
                    DropCasingManually(ModContent.GoreType<ShellBuckShotGore>());
                    break;
            }

            switch (PumpActionTimer)
            {
                case 4:
                    SoundEngine.PlaySound(PumpForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 14:
                    Projectile.frame = (int)Insurgency.MagazineState.Fired;
                    break;

                case 28:
                    SoundEngine.PlaySound(PumpBack, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    DropCasingManually(ModContent.GoreType<ShellBuckShotGore>());
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<M590>())
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