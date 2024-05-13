using InsurgencyWeapons.Gores.Casing;
using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using InsurgencyWeapons.Items.Weapons.MachineGuns;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace InsurgencyWeapons.Projectiles
{
    public abstract class WeaponBase : ModProjectile
    {
        /// <summary>
        /// Current ammo in magazine
        /// </summary>
        public virtual int CurrentAmmo { get; set; }

        /// <summary>
        /// In lite mode, weapons reload almost instantly
        /// </summary>
        public bool LiteMode => InsurgencyModConfig.Instance.LiteMode;

        /// <summary>
        /// Weapon will be invisible when not in use
        /// </summary>
        public bool HideWhenNotInUse => InsurgencyModConfig.Instance.HideWhenNotInUse;

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
        /// Max amount of ammo per reload
        /// </summary>
        public int MagazineSize { get; set; }

        /// <summary>
        /// Used for weapon spread
        /// </summary>
        private float Degree { get; set; }

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
        public static int NormalBullet => Insurgency.Bullet;

        /// <summary>
        /// Shorthand for Insurgency.Pellet
        /// </summary>
        public static int ShotgunPellet => Insurgency.Pellet;

        public int BulletDamage
        {
            get
            {
                Item SampleAmmo = ContentSamples.ItemsByType[AmmoType];
                if (InsurgencyModConfig.Instance.DamageScaling)
                    return (int)((Projectile.damage + Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(SampleAmmo.damage)) * Player.GetStealth() * Insurgency.WeaponScaling());

                return (int)((Projectile.damage + Player.GetTotalDamage(DamageClass.Ranged).ApplyTo(SampleAmmo.damage)) * Player.GetStealth());
            }
        }

        public bool ReloadStarted { get; set; }
        public bool ManualReload { get; set; }
        public bool CanFire => ShotDelay >= HeldItem.useTime && !Player.noItems && !Player.CCed;

        private bool ManualReloadCheck()
        {
            return
                Insurgency.Revolvers.Contains(HeldItem.type) ||
                Insurgency.LightMachineGuns.Contains(HeldItem.type) ||
                Insurgency.Rifles.Contains(HeldItem.type);
        }

        public bool CanManualReload(int CurrentAmmo)
        {
            if (ManualReloadCheck() && !(HeldItem.type == ModContent.ItemType<RPK>()))
                return CurrentAmmo != 0 && CurrentAmmo != MagazineSize;

            return CurrentAmmo != 0 && CurrentAmmo != MagazineSize + 1;
        }

        /// <summary>
        /// Player.channel and CurrentAmmo > 0 and ReloadTimer == 0 and CanFire;
        /// </summary>
        /// <param name="CurrentAmmo"></param>
        /// <returns></returns>
        public bool AllowedToFire(int CurrentAmmo) => Player.channel && CurrentAmmo > 0 && ReloadTimer == 0 && CanFire;

        /// <summary>
        /// Ammo != null and Player.HasItem(Ammo.type) and Player.CountItem(Ammo.type) > minAmmo;
        /// </summary>
        /// <param name="minAmmo"></param>
        /// <returns></returns>
        public bool CanReload(int minAmmo = 0) => Ammo != null && Player.HasItem(Ammo.type) && Player.CountItem(Ammo.type) > minAmmo;

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
        /// For lazy people who like to hold left mouse
        /// </summary>
        public int AutoAttack { get; set; }

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
            isPistol, isASmallSprite, BigSpriteSpecificIdlePos, isIdle,
            MouseRightPressed,
            HasUnderBarrelGrenadeLauncer;

        public int ammoStackCount;

        public float drawScale;

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
        /// Returns increasing int until cap that gets multiplied
        /// </summary>
        /// <param name="multiplier"></param>
        /// <param name="maxDegree"></param>
        /// <returns></returns>
        public Vector2 WeaponFireSpreadCalc(int maxDegree, bool shotgun = false)
        {
            bool shouldNotIncrease = Player.scope && MouseRightPressed;
            if (shouldNotIncrease)
                Degree = 0;

            if (!shouldNotIncrease && Player.channel && Main.rand.NextBool(5))
                Degree += 1;

            if (Degree > maxDegree)
                Degree = maxDegree;

            if (shotgun)
                return Player.MountedCenter.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(6, 10))) * HeldItem.shootSpeed;

            return Player.MountedCenter.DirectionTo(MouseAim).RotatedByRandom(MathHelper.ToRadians(Degree)) * HeldItem.shootSpeed;
        }

        /// <summary>
        /// Draws ammo counter near the mouse
        /// </summary>
        /// <param name="CurrentAmmo"></param>
        /// <param name="AmmoType"></param>
        /// <param name="hasGL"></param>
        /// <param name="GrenadeName"></param>
        /// <param name="AmmoTypeGL"></param>
        public void ShowAmmoCounter(int CurrentAmmo, int AmmoType, bool hasGL = false, string GrenadeName = "", int AmmoTypeGL = -1)
        {
            MagazineTracking.BuildUI(CurrentAmmo, AmmoType, hasGL, AmmoTypeGL, GrenadeName);
        }

        public void DropMagazine(int type)
        {
            if (!InsurgencyModConfig.Instance.DropMagazine)
                return;

            BetterNewProjectile(
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
        /// <param name="maxDegree"></param>
        /// <param name="dropCasing"></param>
        /// <param name="ai0"></param>
        /// <param name="ai1"></param>
        /// <param name="ai2"></param>
        /// <param name="shotgun"></param>
        ///
        ///
        ///
        public void Shoot(int maxDegree, bool dropCasing = true, float ai0 = 0, float ai1 = 0, float ai2 = 0, bool shotgun = false)
        {
            float knockBack = Player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(HeldItem.knockBack);
            if (HeldItem.ModItem is not null and Rifle || HeldItem.ModItem is not null and Shotgun)
                knockBack *= 1.175f;

            if (HeldItem.ModItem is not null and SniperRifle)
                knockBack *= 1.33f;

            Vector2 aim = WeaponFireSpreadCalc(maxDegree, shotgun);

            int type = NormalBullet;
            if (shotgun)
                type = ShotgunPellet;

            //Bullet
            Projectile Shot = BetterNewProjectile(
               Player,
               spawnSource: Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo),
               position: Player.MountedCenter,
               velocity: aim,
               type: type,
               damage: BulletDamage,
               knockback: knockBack,
               owner: Player.whoAmI,
               ai0: Projectile.GetGlobalProjectile<ProjPerkTracking>().Perk,
               ai1: ai1,
               ai2: ai2);
            Shot.GetGlobalProjectile<ProjPerkTracking>().ShotFromInsurgencyWeapon = true;

            if (dropCasing)
                DropCasingManually();
        }

        public override void SetDefaults()
        {
            drawScale = 0.9f;
            if (MagazineSize == 0)
                throw new ArgumentException("MagazineSize property can't be 0");

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

        private void DrawWeapon(Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
            Vector2 height = new(rect.Height);
            BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, drawScale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (HideWhenNotInUse && !isIdle)
            {
                DrawWeapon(lightColor);
                return false;
            }
            else if (!HideWhenNotInUse)
                DrawWeapon(lightColor);
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        public override bool PreAI()
        {
            Projectile.CheckPlayerActiveAndNotDead(Player);
            if (Player.HasItem(AmmoType))
            {
                Ammo = Player.FindItemInInventory(AmmoType);
                if (Ammo != null && Ammo.stack <= 0)
                    Ammo.TurnToAir(true);
            }
            if (Player.HasItem(GrenadeLauncherAmmoType))
            {
                AmmoGL = Player.FindItemInInventory(GrenadeLauncherAmmoType);
                if (AmmoGL != null && AmmoGL.stack <= 0)
                    AmmoGL.TurnToAir(true);
            }

            //Muzzleflash light
            if (ShotDelay == 1)
                Lighting.AddLight(Player.Center, Color.Gold.ToVector3());

            //Resetting fields
            #region
            if (ShotDelay <= HeldItem.useTime)
                ShotDelay++;

            if (ReloadTimer > 0)
                ReloadTimer--;

            if (AlternateFireCoolDown > 0)
                AlternateFireCoolDown--;

            if (BoltActionTimer > 0)
                BoltActionTimer--;

            if (PumpActionTimer > 0)
                PumpActionTimer--;

            if (!Player.channel)
                Degree = 0;

            if (AutoAttack > 0)
                AutoAttack--;
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

            int mouseDirection = Player.DirectionTo(MouseAim).X > 0f ? 1 : -1;
            if (Player.channel
                || ReloadTimer > 0
                || UnderAlternateFireCoolDown
                || BoltActionTimer > 0
                || PumpActionTimer > 0)
            {
                isIdle = false;
                recoil = Player.MountedCenter.DirectionFrom(MouseAim) * (ShotDelay / 3f);
                Vector2 distance = (Player.MountedCenter.DirectionTo(MouseAim) * OffsetFromPlayerCenter) - recoil + SpecificWeaponFix;
                Projectile.Center = Player.MountedCenter + distance;
                Projectile.position.Y += Player.gfxOffY;
                Projectile.velocity = Vector2.Zero;
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
                Projectile.rotation = Player.direction == -1 ? -MathHelper.PiOver4 : MathHelper.PiOver4;
                if (isPistol)
                {
                    int X = Player.direction == -1
                        ? 2 //true
                        : -22; //false

                    idlePos = new Vector2(X, -10);
                    Projectile.rotation = Player.direction == -1 ? -MathHelper.Pi : MathHelper.Pi;
                }
                if (isASmallSprite)
                {
                    int X = Player.direction == -1
                        ? -12 //true
                        : -16; //false

                    idlePos = new Vector2(X, -25);
                }
                if (BigSpriteSpecificIdlePos)
                {
                    int X = Player.direction == -1
                       ? -15 //true
                       : -24; //false

                    idlePos = new Vector2(X, -40);
                    Projectile.rotation = Player.direction == -1 ? -MathHelper.Pi : MathHelper.Pi;
                }
                Projectile.position = Player.Center + idlePos;
                Projectile.position.Y += Player.gfxOffY;
                Projectile.spriteDirection = Player.direction;
                if (isPistol)
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

        public void ReloadShotgun(int setReload)
        {
            ammoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, 1);
            Player.ConsumeMultiple(ammoStackCount, Ammo.type);
            CurrentAmmo += ammoStackCount;
            ReloadTimer = setReload;
        }

        public int ReloadMagazine(bool noChamber = false)
        {
            ammoStackCount = Math.Clamp(Player.CountItem(Ammo.type), 1, MagazineSize);
            if (ManualReload)
            {
                if (!noChamber)
                    ammoStackCount++;
                Player.ConsumeMultiple(ammoStackCount, Ammo.type);
                CurrentAmmo = ammoStackCount;
            }
            else
            {
                Player.ConsumeMultiple(ammoStackCount, Ammo.type);
                CurrentAmmo = ammoStackCount;
            }
            return CurrentAmmo;
        }

        /// <summary>
        /// Returns ammo in the chamber to the stack
        /// </summary>
        /// <param name="CurrentAmmo"></param>
        public void ReturnAmmo()
        {
            if (CurrentAmmo == 0)
                return;
            ammoStackCount = CurrentAmmo;
            Ammo.stack += ammoStackCount;
        }
    }
}