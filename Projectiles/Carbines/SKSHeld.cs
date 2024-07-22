using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Carbines;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Carbines;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Carbines
{
    public class SKSHeld : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.SKSMagazine;
            }
            set
            {
                MagazineTracking.SKSMagazine = value;
            }
        }

        private bool SemiAuto;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/sks/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/sks/empty");
        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/sks/bltbk");
        private SoundStyle BoltRel => new("InsurgencyWeapons/Sounds/Weapons/Ins2/sks/bltrel");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/sks/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/sks/magout");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 78;
            MagazineSize = 20;
            drawScale = 0.8f;
            AmmoType = ModContent.ItemType<Bullet762>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height - 30);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.SKSMagazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 14f;
            //SpecificWeaponFix = new Vector2(0, -0.5f);
            if (!Player.channel || AutoAttack == 0)
            {
                SemiAuto = false;
                AutoAttack = (int)(HeldItem.useTime * 1.5f);
            }

            if (AllowedToFire(CurrentAmmo) && !UnderAlternateFireCoolDown && !SemiAuto)
            {
                SemiAuto = true;
                CurrentAmmo--;
                ShotDelay = 0;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(4);
            }

            if (LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadStarted = true;
                ReloadTimer = 14;
            }

            if (!LiteMode && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Carbines;
                ReloadStarted = true;
            }

            if (CurrentAmmo == 0 && Player.channel && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CanManualReload(CurrentAmmo))
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Carbines;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(BoltRel, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine();
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 20:
                    if (!ManualReload)
                    {
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                        SoundEngine.PlaySound(BoltRel, Projectile.Center);
                    }
                    break;

                case 40:
                    if (!ManualReload)
                        SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    break;

                case 80:
                    if (!ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    else
                        Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    if (CanReload())
                        ReloadMagazine();
                    break;

                case 140:
                    if (!ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    else
                        Projectile.frame = 4;

                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    ammoStackCount = CurrentAmmo;
                    Ammo.stack += ammoStackCount;
                    CurrentAmmo = 0;
                    if (!ManualReload)
                        DropMagazine(ModContent.ProjectileType<SKSMagazine>());
                    break;
            }

            if (CurrentAmmo != 0 && ReloadTimer == 0)
            {
                if (ShotDelay <= 2)
                    Projectile.frame = ShotDelay;
                else
                    Projectile.frame = 0;
            }

            if (HeldItem.type != ModContent.ItemType<SKS>())
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