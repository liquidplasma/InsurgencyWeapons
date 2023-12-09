using InsurgencyWeapons.Gores.Casing;
using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Projectiles
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
        /// Used for weapon spread
        /// </summary>
        private int Degree { get; set; }
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

        /// <summary>
        /// Shorthand for Insurgency.Bullet
        /// </summary>
        public int NormalBullet => Insurgency.Bullet;

        public int BulletDamage
        {
            get
            {
                Ammo ??= ContentSamples.ItemsByType[AmmoType];
                if (InsurgencyModConfig.Instance.DamageScaling)
                    return (int)((Projectile.originalDamage + Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Ammo.damage)) * Player.GetStealth() * Insurgency.WeaponScaling());

                return (int)((Projectile.originalDamage + Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Ammo.damage)) * Player.GetStealth());
            }
        }

        public bool ReloadStarted { get; set; }
        public bool CanFire => ShotDelay >= HeldItem.useTime && !Player.noItems && !Player.CCed;

        public bool CanReload(int minAmmo = 0) => Ammo != null && Ammo.stack > minAmmo;

        private bool underAlternateFireCoolDown;

        public bool UnderAlternateFireCoolDown
        {
            get => AlternateFireCoolDown > PercentageOfAltFireCoolDown;
            set => underAlternateFireCoolDown = value;
        }

        /// <summary>
        /// For bolt action snipers rifles
        /// </summary>
        public int BoltActionTimer { get; set; }

        /// <summary>
        /// For pump action shotguns
        /// </summary>
        public int PumpActionTimer { get; set; }

        public Vector2
           recoilVertical, recoil,
           MouseAim,
           idlePos;

        /// <summary>
        /// Ultra jank
        /// </summary>
        public Vector2 SpecificWeaponFix = new(0, 0);

        public bool
            isPistol, isASmallSprite, isIdle,
            MouseRightPressed,
            HasUnderBarrelGrenadeLauncer;

        public int AmmoStackCount;

        /// <summary>
        /// Time in ticks
        /// </summary>
        public int ShotDelay
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        /// <summary>
        /// Time in ticks
        /// </summary>
        public int ReloadTimer
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        /// <summary>
        /// Time in ticks
        /// </summary>
        public int AlternateFireCoolDown
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        /// <summary>
        /// Draws muzzleflash (Cancelled until I can position it better)
        /// </summary>
        /// <param name="color">The color of the muzzleflash</param>
        /// <param name="offset">The offset</param>
        /// <param name="scale">The scale</param>
        public void DrawMuzzleFlash(Color color, float offset, float scale, Vector2 jankFix)
        {
            /*if (ShotDelay <= HeldItem.useTime && Player.channel && !UnderAlternateFireCoolDown)
            {
                Vector2 position = Player.MountedCenter + jankFix;
                Vector2 muzzleDrawPos = position - recoil + recoilVertical;
                float sin = (float)Math.Sin(muzzleDrawPos.AngleTo(MouseAim));
                muzzleDrawPos += muzzleDrawPos.DirectionTo(MouseAim) * (offset - sin);
                Texture2D muzzleFlash = HelperStats.MuzzleFlash;
                Rectangle rect = muzzleFlash.Frame(verticalFrames: 6, frameY: Math.Clamp(ShotDelay, 0, 6));
                ExtensionMethods.BetterEntityDraw(muzzleFlash, muzzleDrawPos, rect, color, Projectile.rotation + MathHelper.PiOver2 * -Player.direction, rect.Size() / 2, scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            }*/
        }
        /// <summary>
        /// Returns increasing int until cap that gets multiplied
        /// </summary>
        /// <param name="multiplier"></param>
        /// <param name="maxDegree"></param>
        /// <returns></returns>
        public float AutomaticWeaponFireSpreadCalc(float multiplier, int maxDegree)
        {
            bool shouldNotIncrease = Player.scope && MouseRightPressed;
            if(shouldNotIncrease)            
                return 0;
            
            if(!shouldNotIncrease && Degree < maxDegree && Player.channel && Main.rand.NextBool(5))            
                Degree += 1;

            return Degree * multiplier;
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
            bool overGrave = Main.tile[MouseAim.ToTileCoordinates()].TileType == 85;
            if (!overGrave && Player.whoAmI == Main.myPlayer && !Player.mouseInterface)
            {
                if (hasGL && ammoTypeGL != -1)
                    Main.instance.MouseText(CurrentAmmo + " / " + Player.CountItem(AmmoType) + grenadeName + Player.CountItem(ammoTypeGL));
                else
                    Main.instance.MouseText(CurrentAmmo + " / " + Player.CountItem(AmmoType));
            }
        }

        public void DropMagazine(int type)
        {
            if (!InsurgencyModConfig.Instance.DropMagazine)
                return;

            ExtensionMethods.BetterNewProjectile(
            Player,
            Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo),
            Projectile.Center,
            new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), -Main.rand.NextFloat(1f, 1.5f)).RotatedByRandom(MathHelper.PiOver4),
            type,
            0,
            0f,
            Player.whoAmI);
        }

        public void DropCasingManually(int type = 0)
        {
            if (!InsurgencyModConfig.Instance.DropCasing)
                return;

            if (type == 0)
                type = ModContent.GoreType<CasingGore>();

            Gore.NewGoreDirect(Player.GetSource_ItemUse(HeldItem), Player.MountedCenter + Player.MountedCenter.DirectionTo(MouseAim) * OffsetFromPlayerCenter, new Vector2(0, -Main.rand.NextFloat(2f, 3f)).RotatedByRandom(MathHelper.PiOver4), type);
        }

        /// <summary>
        /// Shoot boolet
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        /// <param name="dropCasing"></param>
        public void Shoot(Vector2 aim, int type, int damage, bool dropCasing = true, float ai0 = 0)
        {
            float knockBack = Player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(HeldItem.knockBack);
            if (HeldItem.ModItem is not null and Rifle || HeldItem.ModItem is not null and Shotgun)
                knockBack *= 1.175f;

            if (HeldItem.ModItem is not null and SniperRifle)
                knockBack *= 1.33f;

            //Bullet
            ExtensionMethods.BetterNewProjectile(
               Player,
               spawnSource: Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo),
               position: Player.MountedCenter,
               velocity: aim,
               type: type,
               damage: damage,
               knockback: knockBack,
               owner: Player.whoAmI,
               ai0: ai0);

            if (dropCasing)
                DropCasingManually();
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

        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        public override bool PreAI()
        {
            Projectile.CheckPlayerActiveAndNotDead(Player);

            //Check if ammo is 0 or less then remove it if necessary, since I'm directly reducing stack size
            if (Ammo != null && Ammo.stack <= 0)
                Ammo.TurnToAir();
            if (AmmoGL != null && AmmoGL.stack <= 0)
                AmmoGL.TurnToAir();

            //Muzzleflash light
            if (ShotDelay == 1)
            {
                Lighting.AddLight(Player.Center, Color.Gold.ToVector3());
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
            if (BoltActionTimer > 0)
            {
                BoltActionTimer--;
            }
            if (PumpActionTimer > 0)
            {
                PumpActionTimer--;
            }
            if (!Player.channel)
            {
                Degree = 0;
            }
            #endregion

            return base.PreAI();
        }

        public override void AI()
        {
            //Give scope to player if he is holding sniper rifles
            if (Insurgency.SniperRifles.Contains(HeldItem.type))
                Player.scope = true;

            if (Projectile.active)
                MagazineTracking.isActive = true;

            if (Player.whoAmI == Main.myPlayer)
            {
                MouseAim = Main.MouseWorld;
                if (!Player.mouseInterface)
                    MouseRightPressed = Main.mouseRight;

                Projectile.netUpdate = true;
            }

            //positioning / velocity
            #region
            idlePos = new Vector2(-12, -40);
            if (isPistol)
            {
                int X = Player.direction == -1
                    ? 2 //true
                    : -22; //false

                idlePos = new Vector2(X, -10);
            }
            if (isASmallSprite)
            {
                int X = Player.direction == -1
                    ? -12 //true
                    : -16; //false

                idlePos = new Vector2(X, -20);
            }

            if (Player.channel
                || ReloadTimer > 0
                || UnderAlternateFireCoolDown
                || BoltActionTimer > 0
                || PumpActionTimer > 0)
            {
                isIdle = false;
                recoil = Player.MountedCenter.DirectionFrom(MouseAim) * (ShotDelay / 3f);

                Vector2 distance = (Player.MountedCenter.DirectionTo(MouseAim) * OffsetFromPlayerCenter) - recoil + SpecificWeaponFix;
                Projectile.Center = Player.MountedCenter + distance - new Vector2(0f, Projectile.gfxOffY - Player.gfxOffY);
                Projectile.velocity = Vector2.Zero;
                int mouseDirection = Player.DirectionTo(MouseAim).X > 0f ? 1 : -1;
                Projectile.rotation = Player.AngleTo(MouseAim) + MathHelper.PiOver2;
                Player.ChangeDir(mouseDirection);
                Projectile.spriteDirection = Player.direction;
                Player.heldProj = Projectile.whoAmI;
                Player.HoldOutArm(Projectile, MouseAim);
                Player.SetDummyItemTime(2);
            }
            else if (ReloadTimer == 0)
            {
                isIdle = true;
                Projectile.position = Player.Center + idlePos;
                Projectile.spriteDirection = Player.direction;
                Projectile.rotation = Player.direction == -1 ? -MathHelper.PiOver4 : MathHelper.PiOver4;
                if (isPistol)
                    Projectile.rotation = Player.direction == -1 ? -MathHelper.Pi : MathHelper.Pi;

                Projectile.frame = 0;
            }
            #endregion
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(MouseAim);
            writer.Write(MouseRightPressed);
            writer.Write(UnderAlternateFireCoolDown);
            writer.Write(PercentageOfAltFireCoolDown);
            writer.Write(AlternateFireCoolDown);
            writer.Write(BoltActionTimer);
            writer.Write(PumpActionTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MouseAim = reader.ReadVector2();
            MouseRightPressed = reader.ReadBoolean();
            UnderAlternateFireCoolDown = reader.ReadBoolean();
            PercentageOfAltFireCoolDown = reader.ReadSingle();
            AlternateFireCoolDown = reader.ReadInt32();
            BoltActionTimer = reader.ReadInt32();
            PumpActionTimer = reader.ReadInt32();
        }
    }
}