using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Revolvers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.Revolvers
{
    internal class ColtPythonHeld : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.PythonCylinder;
            }
            set
            {
                MagazineTracking.PythonCylinder = value;
            }
        }

        private int FireDelay { get; set; }
        private bool AllowedToFire => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/python/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private bool SemiAuto;

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/python/empty");
        private SoundStyle Dump => new("InsurgencyWeapons/Sounds/Weapons/Ins2/python/dump", 2);
        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/ins");

        private SoundStyle Hammer => new("InsurgencyWeapons/Sounds/Weapons/Ins2/python/hammer");
        private SoundStyle Open => new("InsurgencyWeapons/Sounds/Weapons/Ins2/python/open");
        private SoundStyle Close => new("InsurgencyWeapons/Sounds/Weapons/Ins2/python/close");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 36;
            MaxAmmo = 6;
            AmmoType = ModContent.ItemType<Bullet357>();
            isPistol = true;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float scale;
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            if (isIdle)
                scale = 0.75f;
            else
                scale = 1f;
            ExtensionMethods.BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.Yellow, 45f, 1f, new Vector2(0, -7f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.PythonCylinder;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            Ammo ??= Player.FindItemInInventory(AmmoType);
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 10f;
            if (!Player.channel)
                SemiAuto = false;

            if (Player.channel && !SemiAuto)
            {
                FireDelay++;
                if (FireDelay == 4)
                    SoundEngine.PlaySound(Hammer, Projectile.Center);
            }
            else
                FireDelay = 0;

            if (AllowedToFire && !UnderAlternateFireCoolDown && !SemiAuto && FireDelay >= 8)
            {
                SemiAuto = true;
                ShotDelay = FireDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(1, 2, NormalBullet, BulletDamage, dropCasing: false);
            }

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Revolvers;
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0 && BoltActionTimer == 0)
            {
                BoltActionTimer = 30;
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CanManualReload(CurrentAmmo))
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.Revolvers;
            }

            switch (ReloadTimer)
            {
                case 30:
                    SoundEngine.PlaySound(Close, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = ManualReload = false;
                    break;

                case 120:
                    if (!ManualReload)
                        SoundEngine.PlaySound(Open, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    if (CanReload() && !ManualReload)
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, MaxAmmo);
                        Ammo.stack -= AmmoStackCount;
                        CurrentAmmo = AmmoStackCount;
                    }

                    if (CurrentAmmo < MaxAmmo && CanReload() && ManualReload)
                    {
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        DropCasingManually();
                        Ammo.stack--;
                        CurrentAmmo++;
                        ReloadTimer = 170;
                        if (CurrentAmmo == MaxAmmo)
                            ReloadTimer = 120;
                    }

                    if (!ManualReload)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            DropCasingManually();
                        }
                    }
                    break;

                case 162:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    break;

                case 180:
                    SoundEngine.PlaySound(Dump, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;
            }

            switch (BoltActionTimer)
            {
                case 5:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    break;

                case 10:
                    SoundEngine.PlaySound(Hammer, Projectile.Center);
                    SoundEngine.PlaySound(Empty, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Fired;
                    break;

                case 15:
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;
            }

            if (ShotDelay < HeldItem.useTime)
                Projectile.frame = Math.Clamp(ShotDelay, 0, 2);

            if (HeldItem.type != ModContent.ItemType<ColtPython>())
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