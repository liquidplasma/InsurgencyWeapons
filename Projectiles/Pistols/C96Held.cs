using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Pistols;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Pistols;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Pistols
{
    public class C96Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.C96Clip;
            }
            set
            {
                MagazineTracking.C96Clip = value;
            }
        }

        private bool SemiAuto;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/c96/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/c96/empty");
        private SoundStyle ClipIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/c96/clin");
        private SoundStyle ClipRemove => new("InsurgencyWeapons/Sounds/Weapons/Ins2/c96/clrem");
        private SoundStyle BoltRelease => new("InsurgencyWeapons/Sounds/Weapons/Ins2/c96/bltrel");
        private SoundStyle SlideBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/c96/sldbk");
        private SoundStyle MagSlideForward => new("InsurgencyWeapons/Sounds/Weapons/Ins2/c96/magfdl");
        private SoundStyle Dump => new("InsurgencyWeapons/Sounds/Weapons/Ins2/c96/dump");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 46;
            MagazineSize = 7;
            AmmoType = ModContent.ItemType<Bullet76325>();
            drawScale = 1f;
            isPistol = true;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height - 8);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.C96Clip;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            if (isIdle)
                drawScale = 0.75f;
            else
                drawScale = 1f;
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 12f;
            SpecificWeaponFix = new Vector2(-2 * Player.direction, -4);

            if (!Player.channel || AutoAttack == 0)
            {
                SemiAuto = false;
                AutoAttack = (int)(HeldItem.useTime * 1.5f);
            }

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && !SemiAuto)
            {
                SemiAuto = true;
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(2);
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadStarted = true;
                ReloadTimer = 14;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Rifles;
                ReloadTimer += 25;
                SoundEngine.PlaySound(SlideBack, Projectile.Center);
                Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
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
                ReloadTimer += 90;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(BoltRelease, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine(true);
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 15:
                    SoundEngine.PlaySound(BoltRelease, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 20:
                    SoundEngine.PlaySound(ClipRemove, Projectile.Center);
                    DropMagazine(ModContent.ProjectileType<C96Clip>());
                    break;

                case 60:
                    SoundEngine.PlaySound(ClipIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    if (CanReload())
                        ReloadMagazine(true);
                    break;

                case 78:
                    if (ManualReload)
                        SoundEngine.PlaySound(MagSlideForward, Projectile.Center);
                    break;

                case 118:
                    if (ManualReload)
                        SoundEngine.PlaySound(Dump, Projectile.Center);
                    for (int i = 0; i < CurrentAmmo; i++)
                        DropCasingManually();
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    break;

                case 120:
                    if (ManualReload)
                        SoundEngine.PlaySound(SlideBack, Projectile.Center);

                    ReturnAmmo();
                    break;
            }
            if (CurrentAmmo != 0 && ReloadTimer == 0)
            {
                if (ShotDelay <= 2)
                    Projectile.frame = ShotDelay;
                else
                    Projectile.frame = 0;
            }

            if (HeldItem.type != ModContent.ItemType<C96>())
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