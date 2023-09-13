using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles.WeaponExtras
{
    internal abstract class WeaponBase : ModProjectile
    {
        /// <summary>
        /// The projectile owner of this weapon
        /// </summary>
        public Player Player => Main.player[Projectile.owner];

        /// <summary>
        /// Players current held item
        /// </summary>
        public Item HeldItem => Player.HeldItem;

        /// <summary>
        /// Tracking magazine sizes
        /// </summary>
        public InsurgencyMagazineTracking MagazineTracking => Player.GetModPlayer<InsurgencyMagazineTracking>();

        /// <summary>
        /// The ammo
        /// </summary>
        public Item Ammo { get; set; }

        /// <summary>
        /// The grenade launcher ammo, if used
        /// </summary>
        public Item AmmoGL { get; set; }

        /// <summary>
        /// Max amount of ammo per reload, should be assigned in set defaults
        /// </summary>
        public int MaxAmmo { get; set; }

        /// <summary>
        /// Ammo item type
        /// </summary>
        public int AmmoType { get; set; }

        /// <summary>
        /// Grenade launcher item type
        /// </summary>
        public int GrenadeLauncherAmmoType { get; set; }

        /// <summary>
        /// Time to wait to hide weapon sprite
        /// </summary>
        public float PercentageOfAltFireCoolDown { get; set; }

        /// <summary>
        /// Small jank fix for weird positioning
        /// </summary>
        public float OffsetFromPlayerCenter { get; set; }

        public bool ReloadStarted { get; set; }
        public bool CanFire => ShotDelay >= HeldItem.useTime;
        public bool UnderAlternateFireCoolDown => AlternateFireCoolDown > PercentageOfAltFireCoolDown;

        public Vector2
           recoilVertical, recoil,
           MouseAim,
           idlePos;

        /// <summary>
        /// Ultra jank
        /// </summary>
        public Vector2 SpecificWeaponFix = new(0, 0);

        public bool
            MouseRightPressed,
            HasUnderBarrelGrenadeLauncer;

        public int lessThanMaxAmmo;

        /// <summary>
        /// Time in ticks
        /// </summary>
        public int ShotDelay
        {
            get => (int)Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        /// <summary>
        /// Time in ticks
        /// </summary>
        public int ReloadTimer
        {
            get => (int)Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        /// <summary>
        /// Time in ticks
        /// </summary>
        public int AlternateFireCoolDown
        {
            get => (int)Projectile.localAI[2];
            set => Projectile.localAI[2] = value;
        }

        /// <summary>
        /// Draws muzzleflash
        /// </summary>
        /// <param name="color">The color of the muzzleflash</param>
        /// <param name="offset">The offset</param>
        /// <param name="scale">The scale</param>
        public void DrawMuzzleFlash(Color color, float offset, float scale, Vector2 jankFix)
        {
            if (ShotDelay <= HeldItem.useTime && Player.channel && !UnderAlternateFireCoolDown)
            {
                Vector2 muzzleDrawPos = Player.RotatedRelativePoint(Player.MountedCenter + jankFix + Player.MountedCenter.DirectionTo(MouseAim) * offset) - recoil + recoilVertical;
                Texture2D muzzleFlash = HelperStats.MuzzleFlash;
                Rectangle rect = muzzleFlash.Frame(verticalFrames: 6, frameY: Math.Clamp(ShotDelay,0,6));
                ExtensionMethods.BetterEntityDraw(muzzleFlash, muzzleDrawPos, rect, color, Projectile.rotation + MathHelper.PiOver2 * -Player.direction, rect.Size() / 2, scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            }
        }

        /// <summary>
        /// Draws ammo counter near the mouse
        /// </summary>
        /// <param name="CurrentAmmo"></param>
        /// <param name="AmmoType"></param>
        /// <param name="hasGL"></param>
        /// <param name="grenadeName"></param>
        /// <param name="ammoTypeGL"></param>
        public void ShowAmmoCounter(int CurrentAmmo, int AmmoType, bool hasGL = false, string grenadeName = "", int ammoTypeGL = -1)
        {
            if (Player.whoAmI == Main.myPlayer && !Player.mouseInterface)
            {
                if (hasGL && ammoTypeGL != -1)
                    Main.instance.MouseText(CurrentAmmo + " / " + Player.CountItem(AmmoType) + grenadeName + Player.CountItem(ammoTypeGL));
                else
                    Main.instance.MouseText(CurrentAmmo + " / " + Player.CountItem(AmmoType));
            }
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.scale = 1;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            PercentageOfAltFireCoolDown = AlternateFireCoolDown * 0.85f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }

        public override bool PreAI()
        {
            Projectile.CheckPlayerActiveAndNotDead(Player);
            ///Check if ammo is 0 or less then remove it if necessary, since I'm directly reducing stack size
            if (Ammo != null && Ammo.stack <= 0)
                Ammo.TurnToAir();
            if (AmmoGL != null && AmmoGL.stack <= 0)
                AmmoGL.TurnToAir();
            return base.PreAI();
        }

        public override void AI()
        {
            if (Projectile.active)
            {
                MagazineTracking.isActive = true;
            }

            //Resetting fields
            #region
            if (ShotDelay <= HeldItem.useTime)
            {
                ShotDelay++;
            }
            if (ReloadTimer > 0)
            {
                ReloadTimer--;
            }
            if (AlternateFireCoolDown > 0)
            {
                AlternateFireCoolDown--;
            }
            #endregion

            if (Player.whoAmI == Main.myPlayer)
            {
                MouseAim = Main.MouseWorld;
                if (!Player.mouseInterface)
                {
                    MouseRightPressed = Main.mouseRight;
                }
                Projectile.netUpdate = true;
            }

            //positioning / velocity
            #region
            idlePos = new Vector2(-12, -40);
            if (Player.channel || ReloadTimer > 0 || UnderAlternateFireCoolDown || Insurgency.Rifles.Contains(HeldItem.type))
            {
                recoil = Player.MountedCenter.DirectionFrom(MouseAim) * (ShotDelay / 3f);
                recoilVertical = Vector2.UnitY * (-ShotDelay / 6f);
                Vector2 distance = Player.MountedCenter.DirectionTo(MouseAim) * OffsetFromPlayerCenter - recoil + recoilVertical + SpecificWeaponFix;
                Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter + distance);
                Projectile.velocity = Vector2.Zero;
                int mouseDirection = Player.DirectionTo(MouseAim).X > 0f ? 1 : -1;
                Projectile.rotation = Player.AngleTo(MouseAim) + MathHelper.PiOver2;
                Player.ChangeDir(mouseDirection);
                Projectile.spriteDirection = Player.direction;
                Player.heldProj = Projectile.whoAmI;
                Player.HoldOutArm(Projectile, MouseAim);
                if (!Insurgency.Rifles.Contains(HeldItem.type))
                {
                    Player.SetDummyItemTime(2);
                }
            }
            else if (ReloadTimer == 0)
            {
                Projectile.position = Player.Center + idlePos;
                Projectile.spriteDirection = Player.direction;
                Projectile.rotation = Player.direction == -1 ? -MathHelper.PiOver4 : MathHelper.PiOver4;
                Projectile.frame = 0;
            }
            #endregion
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(MouseAim);
            writer.Write(MouseRightPressed);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MouseAim = reader.ReadVector2();
            MouseRightPressed = reader.ReadBoolean();
        }
    }
}