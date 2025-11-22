using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Pistols;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Pistols;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Pistols
{
    internal class VP70Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.VP70Magazine;
            }
            set
            {
                MagazineTracking.VP70Magazine = value;
            }
        }

        private int autoFireDelay;

        private int burstShotsRemaining;

        private int burstCooldown;

        private bool burstArmed;

        private bool wasChanneling;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/vp70/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/genericempty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/vp70/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/vp70/magout");
        private SoundStyle SlideRel => new("InsurgencyWeapons/Sounds/Weapons/Ins2/vp70/sldrel");
        private SoundStyle SlideBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/vp70/sldbk");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 48;
            MagazineSize = 18;
            AmmoType = ModContent.ItemType<Bullet919>();
            drawScale = 0.667f;
            isPistolSized = true;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height - 14);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            burstShotsRemaining = 0;
            burstCooldown = 0;
            burstArmed = true;
            wasChanneling = false;
            CurrentAmmo = MagazineTracking.VP70Magazine;
            ShotDelay = HeldItem.useTime;
        }

        public override bool PreAI()
        {
            if (burstCooldown > 0)
                burstCooldown--;
            if (autoFireDelay > 0)
                autoFireDelay--;

            return base.PreAI();
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 2f;
            SpecificWeaponFix = new Vector2(0, -2);

            bool justPressed = Player.channel && !wasChanneling;
            wasChanneling = Player.channel;

            if ((justPressed || (Player.channel && autoFireDelay == 0)) && burstArmed && !ReloadStarted && CurrentAmmo > 0)
            {
                burstShotsRemaining = Math.Min(3, CurrentAmmo);
                burstCooldown = 0;
                burstArmed = false;
            }

            if (burstShotsRemaining > 0 && burstCooldown == 0 && AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(12);

                burstShotsRemaining--;
                burstCooldown = Math.Max(2, HeldItem.useTime / 4);

                if (burstShotsRemaining == 0)
                    autoFireDelay = HeldItem.useTime * 4;
            }

            if (burstShotsRemaining == 0 && autoFireDelay == 0)
                burstArmed = true;

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadStarted = true;
                ReloadTimer = 14;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = (int)(HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles * 1.5f);
                ReloadTimer += 90;
                SoundEngine.PlaySound(SlideBack, Projectile.Center);
                Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 5;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CanManualReload(CurrentAmmo))
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = (int)(HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles * 1.5f);
                ReloadTimer += 90;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(SlideRel, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine();
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 15:
                    if (!ManualReload)
                        SoundEngine.PlaySound(SlideRel, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 60:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    if (CanReload())
                        ReloadMagazine();
                    break;

                case 120:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    ReturnAmmo();
                    CurrentAmmo = 0;
                    if (!ManualReload)
                    {
                        DropMagazine(ModContent.ProjectileType<VP70Magazine>());
                        Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
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

            if (HeldItem.type != ModContent.ItemType<VP70>())
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