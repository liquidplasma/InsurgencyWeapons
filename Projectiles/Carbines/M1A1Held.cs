using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Carbines;
using InsurgencyWeapons.Items.Weapons.Shotguns;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Carbines;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles.Carbines
{
    public class M1A1Held : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M1A1Magazine;
            }
            set
            {
                MagazineTracking.M1A1Magazine = value;
            }
        }

        private bool SemiAuto;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1a1p/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1a1p/empty");
        private SoundStyle BoltBack => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1a1p/bltbk");
        private SoundStyle BoltRel => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1a1p/bltrel");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1a1p/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m1a1p/magout");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 86;
            ClipSize = 15;
            AmmoType = ModContent.ItemType<Bullet76233>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, 0.8f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.Yellow, 44f, 1f, new Vector2(0, -4f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M1A1Magazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 14f;
            SpecificWeaponFix = new Vector2(0, -0.5f);
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
                Shoot(1.5f, 4);
            }

            if (CurrentAmmo == 0 && Player.CountItem(AmmoType) > 0 && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Carbines;
                Main.NewText(ReloadTimer);
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CurrentAmmo != 0 && CurrentAmmo != ClipSize)
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Carbines;
            }

            switch (ReloadTimer)
            {
                case 20:
                    if (!ManualReload)
                        SoundEngine.PlaySound(BoltRel, Projectile.Center);
                    ReloadStarted = ManualReload = false;
                    break;

                case 40:
                    if (!ManualReload)
                        SoundEngine.PlaySound(BoltBack, Projectile.Center);
                    break;

                case 80:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = 0;
                    if (CanReload())
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, ClipSize);
                        if (ManualReload)
                        {
                            AmmoStackCount++;
                            Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                            CurrentAmmo = AmmoStackCount;
                        }
                        else
                        {
                            Player.ConsumeMultiple(AmmoStackCount, Ammo.type);
                            CurrentAmmo = AmmoStackCount;
                        }
                    }
                    break;

                case 140:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    Projectile.frame = 1;
                    AmmoStackCount = CurrentAmmo;
                    Ammo.stack += AmmoStackCount;
                    CurrentAmmo = 0;
                    if (!ManualReload)
                    {
                        DropMagazine(ModContent.ProjectileType<M1A1Magazine>());
                    }
                    break;
            }

            if (HeldItem.type != ModContent.ItemType<M1A1>())
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