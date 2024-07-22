using InsurgencyWeapons.Gores.Casing;
using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Shotguns;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Shotguns
{
    public class CoachHeld : WeaponBase
    {
        public override int CurrentAmmo
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

        private int DoubleBarrelMuzzleflash
        {
            get
            {
                return (int)Projectile.localAI[0];
            }
            set
            {
                Projectile.localAI[0] = value;
            }
        }

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
            MagazineSize = 2;
            drawScale = 0.9f;
            isShotgun = true;
            AmmoType = ModContent.ItemType<ShellBuck_Ball>();
            base.SetDefaults();
        }

        public override void DrawMuzzleFlash(Color color, float scale, float distance)
        {
            Vector2 offset = Player.MountedCenter.DirectionTo(MouseAim) * distance;
            bulletPos = muzzlePos;
            if (ShotDelay <= HeldItem.useTime && Player.channel && !UnderAlternateFireCoolDown)
            {
                Rectangle rect = MuzzleFlash.Frame(verticalFrames: 6, frameY: Math.Clamp(ShotDelay, 0, 6));
                BetterEntityDraw(MuzzleFlash, muzzlePos + offset, rect, color, Projectile.rotation + MathHelper.PiOver2 * -Player.direction, rect.Size() / 2, scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            }
            if (DoubleBarrelMuzzleflash > 0)
            {
                Rectangle rect = MuzzleFlash.Frame(verticalFrames: 6, frameY: 6 - DoubleBarrelMuzzleflash);
                BetterEntityDraw(MuzzleFlash, muzzlePos + offset, rect, color, Projectile.rotation + MathHelper.PiOver2 * -Player.direction, rect.Size() / 2, scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            float scale;
            if (isIdle)
                scale = 0.75f;
            else
                scale = 0.9f;
            BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.PaleGoldenrod, 2f, Projectile.height - 8);
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.CoachBarrel;
            ShotDelay = HeldItem.useTime;
            PercentageOfAltFireCoolDown = 180 * 0.85f;
        }

        public override bool PreAI()
        {
            if (DoubleBarrelMuzzleflash > 0)
                DoubleBarrelMuzzleflash--;
            return base.PreAI();
        }

        public override void AI()
        {
            if (isIdle)
                drawScale = 0.75f;
            else
                drawScale = 0.9f;
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 12f;
            SpecificWeaponFix = new Vector2(0, -2);
            if (!Player.channel)
                SemiAuto = false;

            bool CanFireBothBarrels = MouseRightPressed && CanFire && CurrentAmmo == 2 && ReloadTimer == 0;
            if ((AllowedToFire(CurrentAmmo) || CanFireBothBarrels) && !UnderAlternateFireCoolDown && !SemiAuto)
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
                    Player.velocity += Player.Center.DirectionFrom(MouseAim) * 3f;
                    DoubleBarrelMuzzleflash = 6;
                }

                for (int i = 0; i < both; i++)
                {
                    CurrentAmmo--;
                    for (int j = 0; j < 6; j++)
                    {
                        //Buck
                        Shoot(1, dropCasing: false);
                    }

                    //Ball
                    Shoot(1, dropCasing: false);
                }
            }

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 200;
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
                ReloadTimer = 200;
            }

            switch (ReloadTimer)
            {
                case 15:
                    SoundEngine.PlaySound(Close, Projectile.Center);
                    Projectile.frame = (int)CoachState.Ready;
                    ReloadStarted = ManualReload = false;
                    break;

                case 108:
                    if (CanReload())
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        ammoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                        Player.ConsumeMultiple(ammoStackCount, Ammo.type);
                        CurrentAmmo += ammoStackCount;
                    }
                    break;

                case 132:
                    if (!ManualReload && CanReload())
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        ammoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
                        Player.ConsumeMultiple(ammoStackCount, Ammo.type);
                        CurrentAmmo += ammoStackCount;
                    }
                    break;

                case 162:
                    SoundEngine.PlaySound(Eject, Projectile.Center);
                    if (!ManualReload)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            DropCasingManually(type: ModContent.GoreType<ShellBuckBallGore>());
                        }
                    }
                    else
                        DropCasingManually(type: ModContent.GoreType<ShellBuckBallGore>());
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