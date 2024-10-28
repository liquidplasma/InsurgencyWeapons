using InsurgencyWeapons.Gores.Casing;
using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.MachineGuns;
using InsurgencyWeapons.Projectiles.AssaultRifles;
using InsurgencyWeapons.Projectiles.Pistols;
using InsurgencyWeapons.Projectiles.SubMachineGuns;
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

        public InsurgencyCustomSetBonusModPlayer SetBonusTracking => Player.GetModPlayer<InsurgencyCustomSetBonusModPlayer>();

        /// <summary>
        /// Tracking global projectile
        /// </summary>
        public InsurgencyGlobalProjectile GlobalProjectileTracking => Projectile.GetGlobalProjectile<InsurgencyGlobalProjectile>();

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
        public float Degree { get; set; }

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

        /// <summary>
        /// Muzzleflash texture
        /// </summary>
        public Texture2D MuzzleFlash => ModContent.Request<Texture2D>("InsurgencyWeapons/Textures/Muzzleflash").Value;

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
                Insurgency.Rifles.Contains(HeldItem.type) ||
                this is C96Held;
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
           MouseAim, muzzlePos, bulletPos,
           idlePos;

        /// <summary>
        /// Ultra jank
        /// </summary>
        public Vector2 SpecificWeaponFix = new(0, 0);

        public bool
            isPistol, isShotgun, isASmallSprite, BigSpriteSpecificIdlePos, isIdle,
            MouseRightPressed,
            HasUnderBarrelGrenadeLauncer;

        public int
            ammoStackCount,
            shotgunSwitchDelay;

        public float drawScale = 0.8f;

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
            if (Player.channel && Main.rand.NextBool(5))
                Degree += 1;

            if (Degree > maxDegree)
                Degree = maxDegree;

            if (shouldNotIncrease || CurrentAmmo == 0)
                Degree = 0;

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

            if (Player.whoAmI == Main.myPlayer)
                Projectile.NewProjectileDirect(
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
        /// <param name="maxDegree">Maximus degree in which the spread of the gun can achieve</param>
        /// <param name="dropCasing"></param>
        /// <param name="ai1"></param>
        /// <param name="ai2"></param>
        public void Shoot(int maxDegree, bool dropCasing = true, float ai1 = 0, float ai2 = 0)
        {
            float knockBack = Player.GetTotalKnockback(DamageClass.Ranged).ApplyTo(HeldItem.knockBack);
            if (HeldItem.ModItem is Rifle || HeldItem.ModItem is Shotgun)
                knockBack *= 1.175f;

            if (HeldItem.ModItem is SniperRifle)
                knockBack *= 1.33f;

            Vector2 aim = WeaponFireSpreadCalc(maxDegree, isShotgun);
            if (AmmoType == ModContent.ItemType<TwelveGaugeSlug>())
                aim = WeaponFireSpreadCalc(0, false);
            int type = isShotgun ? ShotgunPellet : NormalBullet;

            //Bullet
            if (Player.whoAmI == Main.myPlayer)
            {
                Projectile Shot = Projectile.NewProjectileDirect(
                   spawnSource: Player.GetSource_ItemUse_WithPotentialAmmo(HeldItem, HeldItem.useAmmo),
                   position: bulletPos,
                   velocity: aim,
                   type: type,
                   damage: BulletDamage,
                   knockback: knockBack,
                   owner: Player.whoAmI,
                   ai0: Projectile.GetGlobalProjectile<ProjPerkTracking>().Perk,
                   ai1: ai1,
                   ai2: ai2);
                Shot.GetGlobalProjectile<ProjPerkTracking>().ShotFromInsurgencyWeapon = true;
            }
            if (dropCasing)
                DropCasingManually();
        }

        public override void SetDefaults()
        {
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

        public override bool ShouldUpdatePosition()
        {
            return false;
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
            BetterEntityDraw(myTexture, Projectile.Center, rect, lightColor, Projectile.rotation, rect.Size() / 2, drawScale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
        }

        /// <summary>
        /// Draws muzzleflash
        /// </summary>
        /// <param name="color">The color of the muzzleflash</param>
        /// <param name="offset">The offset</param>
        /// <param name="scale">The scale</param>
        public virtual void DrawMuzzleFlash(Color color, float scale, float distance)
        {
            Vector2 offset = Player.MountedCenter.DirectionTo(MouseAim) * distance;
            bulletPos = muzzlePos;
            if (ShotDelay <= HeldItem.useTime && Player.channel && !UnderAlternateFireCoolDown)
            {
                Rectangle rect = MuzzleFlash.Frame(verticalFrames: 6, frameY: Math.Clamp(ShotDelay, 0, 6));
                BetterEntityDraw(MuzzleFlash, muzzlePos + offset, rect, color, Projectile.rotation + MathHelper.PiOver2 * -Player.direction, rect.Size() / 2, scale, (SpriteEffects)(Player.direction > 0 ? 0 : 1), 0);
            }
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
            if (ShotDelay == 0)
            {
                float mult = 1f;
                if (this is ASVALHeld || this is MP5SDHeld)
                    mult = 0.75f;
                Lighting.AddLight(Player.Center, Color.Gold.ToVector3() * mult);
            }

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

            if (shotgunSwitchDelay > 0)
                shotgunSwitchDelay--;
            #endregion

            return base.PreAI();
        }

        public override void AI()
        {
            if (Insurgency.AssaultRifles.Contains(HeldItem.type) || Insurgency.Carbines.Contains(HeldItem.type))
                SpecificWeaponFix = new(0, -2);

            if (Player.whoAmI == Main.myPlayer)
            {
                MouseAim = Main.MouseWorld;
                if (!Player.mouseInterface && !MagazineTracking.MouseOverFriendlyNPC)
                    MouseRightPressed = Main.mouseRight;

                Projectile.netUpdate = true;
            }

            //Give scope to player if he is holding sniper rifles
            if (Insurgency.SniperRifles.Contains(HeldItem.type))
                Player.scope = true;

            if (SetBonusTracking.sniperScope && HeldItem.ModItem is WeaponUtils weapon)
                if (weapon.WeaponPerk == ((int)PerkSystem.Perks.Sharpshooter))
                    Player.scope = true;

            if (Projectile.active)
                MagazineTracking.isActive = true;

            //positioning / velocity
            #region
            idlePos = new Vector2(-12, -40);
            recoil = Player.MountedCenter.DirectionFrom(MouseAim) * (ShotDelay / 3f);
            if (Insurgency.Shotguns.Contains(HeldItem.type) || Insurgency.SniperRifles.Contains(HeldItem.type))
                recoil = Player.MountedCenter.DirectionFrom(MouseAim) * -Math.Clamp((ShotDelay - 12) / -3f, 0, 100);
            Vector2 distance = (Player.MountedCenter.DirectionTo(MouseAim) * OffsetFromPlayerCenter) - recoil;
            muzzlePos = Player.MountedCenter + SpecificWeaponFix + distance;
            int mouseDirection = Player.DirectionTo(MouseAim).X > 0f ? 1 : -1;
            if (Player.channel
                || ReloadTimer > 0
                || UnderAlternateFireCoolDown
                || BoltActionTimer > 0
                || PumpActionTimer > 0)
            {
                isIdle = false;
                Projectile.Center = muzzlePos;
                Projectile.position.Y += Player.gfxOffY;
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

                    Projectile.rotation = Player.direction == -1 ? -MathHelper.Pi : MathHelper.Pi;

                    idlePos = new Vector2(X, -10);
                    if (this is Glock17Held or DeagleHeld)
                    {
                        X = Player.direction == -1
                        ? -16 //true
                        : -36; //false
                        idlePos = new Vector2(X, -Projectile.height / 2.5f);
                    }
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

        /// <summary>
        /// Inserts a single shell
        /// </summary>
        /// <param name="setReload">Tick delay to set ReloadTimer to</param>
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