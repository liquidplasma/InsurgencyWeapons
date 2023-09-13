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
        /// All the rifles goes here
        /// </summary>
        public static List<int> Rifles = new();

        /// <summary>
        /// All of the assault rifles goes here
        /// </summary>
        public static List<int> AssaultRifles = new();

        public enum RiflesEnum
        {
            M1Garand
        }

        public enum AssaultRiflesEnum
        {
            AKM,
            STG44
        }

        public enum ReloadModifiers
        {
            AssaultRifles = 20,
            Rifles = 8
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