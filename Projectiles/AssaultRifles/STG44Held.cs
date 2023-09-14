using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.AssaultRifles;
using InsurgencyWeapons.Projectiles.WeaponMagazines.AssaultRifles;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Casings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.AssaultRifles
{
    internal class STG44Held : WeaponBase
    {
        public int CurrentAmmo
        {
            get
            {
                return MagazineTracking.STGMagazine;
            }
            set
            {
                MagazineTracking.STGMagazine = value;
            }
        }

        private bool AllowedToFire => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/stg44/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        private SoundStyle Empty => new("InsurgencyWeapons/Sounds/Weapons/Ins2/stg44/empty");
        private SoundStyle MagIn => new("InsurgencyWeapons/Sounds/Weapons/Ins2/stg44/magin");
        private SoundStyle MagOut => new("InsurgencyWeapons/Sounds/Weapons/Ins2/stg44/magout");
        private SoundStyle BoltLock => new("InsurgencyWeapons/Sounds/Weapons/Ins2/stg44/bltrel");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 72;
            MaxAmmo = 30;
            AmmoType = ModContent.ItemType<Bullet792>();
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            ExtensionMethods.BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, 0.9f, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            DrawMuzzleFlash(Color.LightYellow, 56f, 1f, new Vector2(0, -3f));
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            CurrentAmmo = MagazineTracking.STGMagazine;
            ShotDelay = HeldItem.useTime;
        }

        public override void AI()
        {
            Ammo = Player.FindItemInInventory(AmmoType);
            Ammo ??= ContentSamples.ItemsByType[AmmoType];
            ShowAmmoCounter(CurrentAmmo, AmmoType);
            OffsetFromPlayerCenter = 8f;
            SpecificWeaponFix = new Vector2(0, 1f);
            if (AllowedToFire)
            {
                ShotDelay = 0;
                CurrentAmmo--;
                SoundEngine.PlaySound(Fire, Projectile.Center);
                Vector2 aim = Projectile.Center.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(-2, 2))) * HeldItem.shootSpeed;
                int damage = (int)((Projectile.damage + Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Ammo.damage)) * Player.GetStealth());
                int type = Ammo.shoot;
                if (type == ProjectileID.Bullet)
                    type = Insurgency.Bullet;
                if (Player.whoAmI == Main.myPlayer)
                {
                    //Bullet
                    Projectile.NewProjectileDirect(
                       spawnSource: Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo),
                       position: Player.MountedCenter + Player.MountedCenter.DirectionTo(Player.Top) * 4f,
                       velocity: aim,
                       type: type,
                       damage: damage,
                       knockback: Player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(HeldItem.knockBack),
                       owner: Player.whoAmI);
                    //Casing
                    Projectile.NewProjectileDirect(
                        spawnSource: Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo),
                        position: Player.MountedCenter + Player.MountedCenter.DirectionTo(Player.Top) * 4f,
                        velocity: new Vector2(0, -Main.rand.NextFloat(2f, 3f)).RotatedByRandom(MathHelper.PiOver4),
                        type: ModContent.ProjectileType<RifleCasing>(),
                        damage: 0,
                        knockback: 0,
                        owner: Player.whoAmI);
                }
            }

            if (CurrentAmmo == 0 && Player.CountItem(Ammo.type) > 0 && !ReloadStarted)
            {
                ReloadTimer = HeldItem.useTime * (int)Insurgency.ReloadModifiers.AssaultRifles;
                ReloadStarted = true;
            }

            if (Player.channel && CanFire && Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(Empty, Projectile.Center);
                Projectile.soundDelay = HeldItem.useTime * 2;
            }

            switch (ReloadTimer)
            {
                case 15:
                    SoundEngine.PlaySound(BoltLock, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.Reloaded;
                    ReloadStarted = false;
                    break;

                case 40:
                    SoundEngine.PlaySound(MagIn, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagIn;

                    if (Ammo.stack > 0)
                    {
                        AmmoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, MaxAmmo);
                        Ammo.stack -= AmmoStackCount;
                        CurrentAmmo = AmmoStackCount;
                    }

                    break;

                case 80:
                    SoundEngine.PlaySound(MagOut, Projectile.Center);
                    Projectile.frame = (int)Insurgency.MagazineState.EmptyMagOut;
                    if (Player.whoAmI == Main.myPlayer)
                        Projectile.NewProjectileDirect(Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo), Projectile.Center, new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), -Main.rand.NextFloat(1f, 1.5f)).RotatedByRandom(MathHelper.PiOver4), ModContent.ProjectileType<STGMagazine>(), 0, 0f, Player.whoAmI);
                    break;
            }

            if (CurrentAmmo > 0 && Player.channel)
            {
                Projectile.frame = Math.Clamp(ShotDelay, 0, 2);
            }

            if (HeldItem.type != ModContent.ItemType<STG44>())
                Projectile.Kill();

            base.AI();
        }
    }
}