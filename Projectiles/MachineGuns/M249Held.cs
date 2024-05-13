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
        public override int CurrentAmmo
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
            drawScale = 0.8f;
            AmmoType = ModContent.ItemType<Bullet556>();
            BigSpriteSpecificIdlePos = true;
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M249Box;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            if (isIdle)
                drawScale = 0.75f;
            else
                drawScale = 0.8f;
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

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadStarted = true;
                ReloadTimer = 14;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 310;
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
                ReloadTimer = 290;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(BoltLock, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine(true);
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 30:
                    if (!ManualReload)
                        SoundEngine.PlaySound(Hit, Projectile.Center);
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
                        ReloadMagazine(true);
                    Projectile.frame = 1;
                    break;

                case 160:
                    SoundEngine.PlaySound(Throw, Projectile.Center);
                    if (!ManualReload)
                        DropMagazine(ModContent.ProjectileType<M249Box>());
                    ReturnAmmo();
                    Projectile.frame = 2;
                    break;

                case 280:
                    SoundEngine.PlaySound(Open, Projectile.Center);
                    Projectile.frame = 1;
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