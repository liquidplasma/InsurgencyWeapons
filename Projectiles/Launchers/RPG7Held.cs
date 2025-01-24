using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Launchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.Warheads;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Launchers
{
    public class RPG7Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get => MagazineTracking.RPG7; set
            {
                MagazineTracking.RPG7 = value;
            }
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpg7/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Load1 => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpg7/load1");
        private SoundStyle Load2 => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpg7/load2");
        private SoundStyle Place => new("InsurgencyWeapons/Sounds/Weapons/Ins2/rpg7/place");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 108;
            MagazineSize = 1;
            AmmoType = ModContent.ItemType<RPGRocket>();
            BigSpriteSpecificIdlePos = true;
            drawScale = 0.75f;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.RPG7;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = -8;
            SpecificWeaponFix = new Vector2(0, -4);
            if (LauncherDelay == 0 && AllowedToFire(CurrentAmmo))
            {
                ShotDelay = 0;
                LauncherDelay = 11;
            }
            switch (LauncherDelay)
            {
                case 1:
                    CurrentAmmo--;
                    SoundEngine.PlaySound(Fire, Projectile.Center);
                    ShootRocket(ModContent.ProjectileType<RPGWarhead>(), 1f);
                    break;
            }

            if (!Player.HasItem(AmmoType) && CurrentAmmo == 0)
                Projectile.frame = 1;

            if (LauncherDelay == 0 && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 240;
                Projectile.frame = 1;
                ReloadStarted = true;
            }
            switch (ReloadTimer)
            {
                case 1:
                    if (CanReload())
                        ReloadMagazine();
                    ReloadStarted = false;
                    break;

                case 90:
                    SoundEngine.PlaySound(Load2, Projectile.Center);
                    break;

                case 120:
                    SoundEngine.PlaySound(Load1, Projectile.Center);
                    Projectile.frame = 0;
                    break;

                case 180:
                    SoundEngine.PlaySound(Place, Projectile.Center);
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<RPG7>())
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