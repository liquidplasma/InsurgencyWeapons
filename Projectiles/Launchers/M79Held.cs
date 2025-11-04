using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Launchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.Warheads;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Launchers;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Launchers
{
    internal class M79Held : WeaponBase
    {
        public override int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M79;
            }
            set
            {
                MagazineTracking.M79 = value;
            }
        }

        private enum M79State
        {
            Ready,

            Open
        }

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m79/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle FireBuck => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m79/shoot1")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Open => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m79/open");
        private SoundStyle Close => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m79/close");

        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m79/ins");
        private SoundStyle Eject => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m79/rem");
        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/genericempty");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 82;
            MagazineSize = 1;
            drawScale = 0.75f;
            BigSpriteSpecificIdlePos = true;
            AmmoType = ModContent.ItemType<Grenade40mm>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, drawScale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.PaleGoldenrod, 2f, Projectile.height);
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M79;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 0f;
            SpecificWeaponFix = new Vector2(0, 2);

            if (AllowedToFire(CurrentAmmo))
            {
                ShotDelay = 0;
                CurrentAmmo--;
                if (AmmoType == ModContent.ItemType<Grenade40mm>())
                {
                    SoundEngine.PlaySound(Fire, Projectile.Center);
                    ShootRocket(ModContent.ProjectileType<Grenade40mmProj>(), 0.75f);
                }
                else if (AmmoType == ModContent.ItemType<Buckshot40mm>())
                {
                    SoundEngine.PlaySound(FireBuck, Projectile.Center);

                    for (int i = 0; i < 20; i++)
                        Shoot(0, false);
                }
            }

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = 185;
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            switch (ReloadTimer)
            {
                case 1:
                    ReloadStarted = false;
                    break;

                case 20:
                    SoundEngine.PlaySound(Close, Projectile.Center);
                    Projectile.frame = (int)M79State.Ready;
                    break;

                case 60:
                    SoundEngine.PlaySound(Insert, Projectile.Center);
                    ReloadMagazine();
                    break;

                case 75:
                    //SoundEngine.PlaySound(Drop, Projectile.Center);
                    break;

                case 120:
                    SoundEngine.PlaySound(Eject, Projectile.Center);
                    switch (Ammo.ModItem)
                    {
                        case Grenade40mm:
                            DropMagazine(ModContent.ProjectileType<Grenade40mmUsed>());
                            break;

                        case Buckshot40mm:
                            DropMagazine(ModContent.ProjectileType<Buckshot40mmUsed>());
                            break;
                    }
                    break;

                case 140:
                    Projectile.frame = (int)M79State.Open;
                    SoundEngine.PlaySound(Open, Projectile.Center);
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<M79>())
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