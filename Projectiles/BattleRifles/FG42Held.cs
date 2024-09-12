using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.BattleRifles;
using InsurgencyWeapons.Projectiles.WeaponMagazines.BattleRifles;
using System.IO;

namespace InsurgencyWeapons.Projectiles.BattleRifles
{
    public class FG42Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.FG42Magazine;
            }
            set
            {
                MagazineTracking.FG42Magazine = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/fg42/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/fg42/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/fg42/magin");
        private SoundStyle MagRel => new("InsurgencyWeapons/Sounds/Weapons/Ins2/fg42/magrel");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/fg42/magout");
        private SoundStyle Hit => new("InsurgencyWeapons/Sounds/Weapons/Ins2/fg42/hit");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 78;
            MagazineSize = 20;
            AmmoType = ModContent.ItemType<Bullet79257>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1.15f, Projectile.height - 32);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.FG42Magazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 10f;
            SpecificWeaponFix = new Vector2(0, -3);
            if (AllowedToFire(CurrentAmmo))
            {
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
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.BattleRifles;
                ReloadTimer += 90;
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
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.BattleRifles;
                ReloadTimer += 90;
                if (LiteMode)
                    ReloadTimer = 14;
            }

            switch (ReloadTimer)
            {
                case 6:
                    if (LiteMode)
                    {
                        SoundEngine.PlaySound(Hit, Projectile.Center);
                        ReturnAmmo();
                        if (CanReload())
                            ReloadMagazine();
                    }
                    ReloadStarted = ManualReload = false;
                    break;

                case 45:
                    if (!ManualReload)
                        SoundEngine.PlaySound(Hit, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;

                case 70:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;

                    if (ManualReload)
                        ReloadTimer = 24;

                    if (CanReload())
                        ReloadMagazine();
                    break;

                case 150:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    if (!ManualReload)
                        Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    ReturnAmmo();
                    CurrentAmmo = 0;
                    if (!ManualReload)
                        DropMagazine(ModContent.ProjectileType<FG42Magazine>());
                    break;

                case 155:
                    SoundEngine.PlaySound(MagRel, Projectile.Center);
                    break;
            }

            if (CurrentAmmo > 0 && Player.channel)
            {
                if (ShotDelay % 2 == 0)
                    Projectile.frame++;
                if (Projectile.frame > 2)
                    Projectile.frame = 0;
            }

            if (HeldItem.type != ModContent.ItemType<FG42>())
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