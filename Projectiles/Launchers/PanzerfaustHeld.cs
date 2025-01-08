using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Launchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.DiscardedLaunchers;
using InsurgencyWeapons.Projectiles.WeaponExtras.Warheads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencyWeapons.Projectiles.Launchers
{
    internal class PanzerfaustHeld : WeaponBase
    {
        public class AT4Held : WeaponBase
        {
            public override int CurrentAmmo
            {
                get => MagazineTracking.Panzerfaust; set
                {
                    MagazineTracking.Panzerfaust = value;
                }
            }

            private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/at4/shoot")
            {
                Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
                MaxInstances = 0,
                Volume = 0.4f
            };

            /// <summary>
            /// 10 frames before Fire
            /// </summary>
            private SoundStyle Latch => new("InsurgencyWeapons/Sounds/Weapons/Ins2/at4/latch");

            public override void SetStaticDefaults()
            {
                Main.projFrames[Type] = 1;
                base.SetStaticDefaults();
            }

            public override void SetDefaults()
            {
                Projectile.width = 36;
                Projectile.height = 108;
                MagazineSize = 1;
                AmmoType = ModContent.ItemType<AT4Rocket>();
                BigSpriteSpecificIdlePos = true;
                drawScale = 0.75f;
                base.SetDefaults();
            }

            public override bool PreDraw(ref Color lightColor)
            {
                DrawMuzzleFlash(Color.Yellow, 1f, Projectile.height - 30);
                if (HelperStats.TestRange(ReloadTimer, 20, 120))
                    return false;
                return base.PreDraw(ref lightColor);
            }

            public override void OnSpawn(IEntitySource source)
            {
                CurrentAmmo = MagazineTracking.Panzerfaust;
                ShotDelay = HeldItem.useTime;
            }

            public override void AI()
            {
                ShowAmmoCounter(CurrentAmmo, AmmoType);
                OffsetFromPlayerCenter = 6f;
                SpecificWeaponFix = new Vector2(0, -1);
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
                        ShootRocket(ModContent.ProjectileType<AT4Warhead>(), 1.5f);
                        break;
                }
                if (LauncherDelay == 0 && CurrentAmmo == 0 && CanReload() && !ReloadStarted)
                {
                    ReloadTimer = 140;
                    ReloadStarted = true;
                }
                switch (ReloadTimer)
                {
                    case 1:
                        if (CanReload())
                            ReloadMagazine();
                        ReloadStarted = false;
                        break;

                    case 20:
                        SoundEngine.PlaySound(Latch, Projectile.Center);
                        break;

                    case 120:
                        DropMagazine(ModContent.ProjectileType<PanzerfaustDiscard>());
                        break;
                }

                if (HeldItem.type != ModContent.ItemType<Panzerfaust>())
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