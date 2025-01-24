using InsurgencyWeapons.Gores.Casing;
using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Shotguns;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Shotguns;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Shotguns
{
    public class Saiga12Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.SaigaMagazine;
            }
            set
            {
                MagazineTracking.SaigaMagazine = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/saiga/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.35f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/saiga/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/saiga/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/saiga/magout");
        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/saiga/bltbk");
        private SoundStyle BoltForward => new("InsurgencyWeapons/Sounds/Weapons/Ins2/saiga/bltrel");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height - 38);
            return base.PreDraw(ref lightColor);
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 98;
            MagazineSize = 20;
            isShotgun = true;
            drawScale = 0.8f;
            BigSpriteSpecificIdlePos = true;
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.SaigaMagazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 14f;
            SpecificWeaponFix = new Vector2(0, -2);
            if (AllowedToFire(CurrentAmmo))
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                bool slug = AmmoType == ModContent.ItemType<TwelveGaugeSlug>();
                if (slug)
                    Shoot(0, casingType: ModContent.GoreType<ShellSlugGore>());
                else
                {
                    for (int j = 0; j < 7; j++)
                    {
                        //Buck
                        Shoot(0, dropCasing: false);
                    }
                    Shoot(0, casingType: ModContent.GoreType<ShellBuckShotGore>());
                }
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadStarted = true;
                ReloadTimer = 14;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * 9;
                ReloadTimer += 30;
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
                ReloadTimer = HeldItem.useTime * 9;
                Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(BoltBack, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine();
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 15:
                    if (!ManualReload)
                        SoundEngine.PlaySound(BoltForward, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 30:
                    if (!ManualReload)
                        SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    break;

                case 70:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    if (ManualReload)
                    {
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                        ReloadTimer = 25;
                    }
                    if (CanReload())
                        ReloadMagazine();
                    break;

                case 110:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    ReturnAmmo();
                    CurrentAmmo = 0;
                    if (!ManualReload)
                        DropMagazine(ModContent.ProjectileType<Saiga12Magazine>());
                    break;
            }

            if (CurrentAmmo > 0 && Player.channel)
                Projectile.frame = Math.Clamp(ShotDelay, 0, 2);

            if (HeldItem.type != ModContent.ItemType<Saiga12>())
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