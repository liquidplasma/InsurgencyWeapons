using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Pistols;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Pistols;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Pistols
{
    public class M1911Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M1911Magazine;
            }
            set
            {
                MagazineTracking.M1911Magazine = value;
            }
        }

        private bool SemiAuto;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1911/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1911/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1911/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1911/magout");
        private SoundStyle SlideRel => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1911/sldrel");
        private SoundStyle SlideBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1911/sldbk");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 34;
            MagazineSize = 7;
            AmmoType = ModContent.ItemType<Bullet45ACP>();
            drawScale = 1f;
            isPistol = true;
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M1911Magazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            if (isIdle)
                drawScale = 0.75f;
            else
                drawScale = 1f;
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 4f;
            SpecificWeaponFix = new Vector2(0, 0);

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
                ReloadTimer += 90;
                SoundEngine.PlaySound(SlideBack, Projectile.Center);
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
                ReloadTimer += 45;
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
                    if (!ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    if (CanReload())
ReloadMagazine();
                    break;

                case 120:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    ReturnAmmo();
                    if (!ManualReload)
                    {
                        DropMagazine(ModContent.ProjectileType<M1911Magazine>());
                        Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
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

            if (HeldItem.type != ModContent.ItemType<M1911>())
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