using InsurgencyWeapons.Items.Other;
using InsurgencyWeapons.Projectiles;
using System.Collections.Generic;

namespace InsurgencyWeapons.Helpers
{
    public static class Insurgency
    {
        public static bool HoldingInsurgencyWeapon(this Player player) => AllWeapons.Contains(player.HeldItem.type);

        /// <summary>
        /// Shorthand for NormalBullet projectile type
        /// </summary>
        public static int Bullet => ModContent.ProjectileType<NormalBullet>();

        /// <summary>
        /// Shorthand for ShotgunPellet projectile type
        /// </summary>
        public static int Pellet => ModContent.ProjectileType<ShotgunPellet>();

        /// <summary>
        /// Shorthand for Money item type
        /// </summary>
        public static int Money => ModContent.ItemType<Money>();

        /// <summary>
        /// All of the ammo types go here
        /// </summary>
        public static List<int> AmmoTypes = new();

        /// <summary>
        /// All of the weapons type go here
        /// </summary>
        public static List<int> AllWeapons = new();

        /// <summary>
        /// All of the assault rifles go here
        /// </summary>
        public static List<int> AssaultRifles = new();

        /// <summary>
        /// All of the battle rifles go here
        /// </summary>
        public static List<int> BattleRifles = new();

        /// <summary>
        /// All of the carbines go here
        /// </summary>
        public static List<int> Carbines = new();

        /// <summary>
        /// All of the handguns go here
        /// </summary>
        public static List<int> Pistols = new();

        /// <summary>
        /// All the rifles go here
        /// </summary>
        public static List<int> Rifles = new();

        /// <summary>
        /// All shotguns go here
        /// </summary>
        public static List<int> Shotguns = new();

        /// <summary>
        /// All sniper rifles go here
        /// </summary>
        public static List<int> SniperRifles = new();

        /// <summary>
        /// All of the revolvers go here
        /// </summary>
        public static List<int> Revolvers = new();

        /// <summary>
        /// All of the SMGs go here
        /// </summary>
        public static List<int> SubMachineGuns = new();

        /// <summary>
        /// All of the LMGs go here
        /// </summary>
        public static List<int> LightMachineGuns = new();

        /// <summary>
        /// All of the grenades go here
        /// </summary>
        public static List<int> Grenades = new();

        /// <summary>
        /// All of the launchers go here
        /// </summary>
        public static List<int> Launchers = new();

        public static float WeaponScaling()
        {
            float modifier = 1f;
            if (Main.hardMode)
            {
                if (NPC.downedPlantBoss)
                    modifier += 0.6f;
                else
                    modifier += 0.2f;
            }
            else if (!Main.hardMode)
                modifier -= 0.1f;

            return Main.masterMode ? modifier * 1.08f : modifier;
        }

        public enum ReloadModifiers
        {
            AssaultRifles = 25,

            BattleRifles = 30,

            Carbines = 23,

            Rifles = 8,

            SniperRifles = 4,

            Revolvers = 23,

            SubMachineGuns = BattleRifles,

            LightMachineGuns = 40
        }

        public enum MagazineState
        {
            Reloaded,

            Fired,

            EmptyMagIn,

            EmptyMagOut,
        }
    }
}