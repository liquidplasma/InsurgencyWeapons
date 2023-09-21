using InsurgencyWeapons.Items.Other;
using InsurgencyWeapons.Projectiles;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Helpers
{
    internal static class Insurgency
    {
        /// <summary>
        /// Shorthand for NormalBullet projectile type
        /// </summary>
        public static int Bullet => ModContent.ProjectileType<NormalBullet>();

        /// <summary>
        /// Shorthand for Money item type
        /// </summary>
        public static int Money => ModContent.ItemType<Money>();

        /// <summary>
        /// All of the ammo types goes here
        /// </summary>
        public static List<int> AmmoTypes = new();

        /// <summary>
        /// All of the weapons type goes here
        /// </summary>
        public static List<int> AllWeapons = new();

        /// <summary>
        /// All of the assault rifles goes here
        /// </summary>
        public static List<int> AssaultRifles = new();

        /// <summary>
        /// All of the carbines goes here
        /// </summary>
        public static List<int> Carbines = new();

        /// <summary>
        /// All the rifles goes here
        /// </summary>
        public static List<int> Rifles = new();

        /// <summary>
        /// All of the revolvers goes here
        /// </summary>
        public static List<int> Revolvers = new();

        /// <summary>
        /// All of the submachineguns goes here
        /// </summary>
        public static List<int> SubMachineGuns = new();

        /// <summary>
        /// All of the grenades goes here
        /// </summary>
        public static List<int> Grenades = new();

        public enum RiflesEnum
        {
            M1Garand = 1
        }

        public enum ReloadModifiers
        {
            AssaultRifles = 20,
            BattleRifles = 30,
            Carbines = 23,
            Rifles = 8,
            Revolvers = 21,
            SubMachineGuns = 30
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