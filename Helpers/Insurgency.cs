using InsurgencyWeapons.Items.Other;
using InsurgencyWeapons.Projectiles;
using System.Collections.Generic;
using Terraria;
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
        /// All of the battle rifles goes here
        /// </summary>
        public static List<int> BattleRifles = new();

        /// <summary>
        /// All of the carbines goes here
        /// </summary>
        public static List<int> Carbines = new();

        /// <summary>
        /// All the rifles goes here
        /// </summary>
        public static List<int> Rifles = new();

        /// <summary>
        /// All sniper rifles goes here
        /// </summary>
        public static List<int> SniperRifles = new();

        /// <summary>
        /// All of the revolvers goes here
        /// </summary>
        public static List<int> Revolvers = new();

        /// <summary>
        /// All of the SMGs goes here
        /// </summary>
        public static List<int> SubMachineGuns = new();

        /// <summary>
        /// All of the LMGs go here
        /// </summary>
        public static List<int> LightMachineGuns = new();

        /// <summary>
        /// All of the grenades goes here
        /// </summary>
        public static List<int> Grenades = new();

        public static float WeaponScaling()
        {
            float modifier = 1f;
            if (Main.hardMode)
            {
                if (NPC.downedPlantBoss)
                    modifier += 0.5f;
                else
                    modifier += 0.15f;
            }
            else if (!Main.hardMode)
            {
                modifier -= 0.5f;
            }
            return modifier;
        }

        public enum APCaliber
        {
            c762x51mm = 1,
            c762x63mm,
            c762x54Rmm,
            c303mm
        }

        public enum ReloadModifiers
        {
            AssaultRifles = 25,
            BattleRifles = 30,
            Carbines = 23,
            Rifles = 8,
            SniperRifles = 4,
            Revolvers = 21,
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