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
    internal class M29Held : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.M29Cylinder;
            }
            set
            {
                MagazineTracking.M29Cylinder = value;
            }
        }

        private bool AllowedToFire => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Open => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/open");
        private SoundStyle Close => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/close");
        private SoundStyle Cock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/cock");
        private SoundStyle Dump => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/dump");
        private SoundStyle Insert => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/ins");
        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/m29/empty");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 36;
            MaxAmmo = 6;
            AmmoType = ModContent.ItemType<Bullet44>();
            isPistol = true;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            ExtensionMethods.BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, 1f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.Yellow, 44f, 1f, new Vector2(0, -4f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.M29Cylinder;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 9f;
            SpecificWeaponFix = new Vector2(0, 0);

            if (AllowedToFire && !UnderAlternateFireCoolDown && BoltActionTimer == 0)
            {
                BoltActionTimer = 60;
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Shoot(1, 0, dropCasing: false);
            }

            if (CurrentAmmo == 0 && CanReload() && !ReloadStarted && BoltActionTimer == 0)
            {
                ReloadTimer = 180;
                Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                ReloadStarted = true;
            }

            if (Player.channel && CurrentAmmo == 0 && CanFire && Projectile.soundDelay == 0 && BoltActionTimer == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            if (Ammo != null && Ammo.stack > 0 && !ReloadStarted && InsurgencyModKeyBind.ReloadKey.JustPressed && CanReload() && CanManualReload(CurrentAmmo))
            {
                ManualReload = true;
                ReloadStarted = true;
                ReloadTimer = 180;
            }

            switch (ReloadTimer)
            {
                case 35:
                    SoundEngine.PlaySound(Close, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = false;
                    break;

                case 70:
                    if (CurrentAmmo < MaxAmmo && CanReload())
                    {
                        if (ManualReload)
                            DropCasingManually();
                        SoundEngine.PlaySound(Insert, Projectile.Center);
                        AmmoStackCount = Math.Clamp(Player.CountItem(AmmoType), 0, 1);
                        Ammo.stack -= AmmoStackCount;
                        CurrentAmmo += AmmoStackCount;
                        ReloadTimer = 100;
                    }
                    break;

                case 125:
                    if (!ManualReload)
                    {
                        SoundEngine.PlaySound(Dump, Projectile.Center);
                        for (int i = 0; i < 6; i++)
                        {
                            DropCasingManually();
                        }
                    }
                    break;

                case 145:
                    SoundEngine.PlaySound(Open, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    break;
            }
            switch (BoltActionTimer)
            {
                case 5:
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;
                    break;

                case 10:
                    Projectile.frame = (int)Insurgency.MagazineState.Fired;
                    break;

                case 15:
                    SoundEngine.PlaySound(Cock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    break;
            }
            if (HeldItem.type != ModContent.ItemType<M29>())
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