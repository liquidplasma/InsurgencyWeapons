﻿using Terraria.Chat;
using Terraria.Localization;

namespace InsurgencyWeapons.Helpers
{
    public static class HelperStats
    {
        /// <summary>
        /// Explosion Smoke
        /// </summary>
        public static int GrenadeGore => Main.rand.Next(61, 64);

        /// <summary>
        /// Dust that looks smokey
        /// </summary>
        public static int SmokeyDust => Utils.SelectRandom(Main.rand, DustID.Smoke, DustID.Torch);

        /// <summary>
        /// Goes from 0f to 1f slowly, MP friendly (probably?)
        /// </summary>
        public static float GlobalTick => Main.GameUpdateCount % 900 / 900f;

        /// <summary>
        /// Returns the ticktime from the seconds
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static int SecondsToTick(int seconds) => seconds * 60;

        public static void SmokeGore(IEntitySource source, Vector2 position, int amount, float magnitudeRange, float rotationRange = MathHelper.TwoPi)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            for (int i = 0; i < amount; ++i)
            {
                Vector2 velocity = Utils.RandomVector2(Main.rand, -magnitudeRange, magnitudeRange).RotatedByRandom(rotationRange);
                Gore.NewGore(source, position, velocity, GrenadeGore);
            }
        }

        public static void SmokeyTrail(Vector2 position, Vector2 velocity)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                int type = Main.rand.NextBool(12) ? DustID.Torch : DustID.Smoke;
                Dust dusty = Dust.NewDustPerfect(position, type);
                dusty.velocity = Vector2.Zero + Utils.NextVector2Circular(Main.rand, -4, 4) * (Main.rand.NextBool() ? 0.1f : 0.33f);
                if (type == DustID.Torch)
                {
                    Dust extraDusty = Dust.NewDustPerfect(position, DustID.SolarFlare);
                    extraDusty.velocity = (velocity * 0.25f) + Vector2.Zero + Utils.NextVector2Circular(Main.rand, -4, 4) * (Main.rand.NextBool() ? 0.2f : 0.53f);
                }
                dusty.fadeIn = 1f;
            }
        }

        public static void Announce(Color color, string text, params string[] substitutions)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text, substitutions), color);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(Language.GetTextValue(text, substitutions), color);
            }
        }

        public static bool TestRange(float numberToCheck, float bottom, float top)
        {
            return numberToCheck >= bottom && numberToCheck <= top;
        }

        public static bool TestRange(int numberToCheck, int bottom, int top)
        {
            return numberToCheck >= bottom && numberToCheck <= top;
        }

        public static float EaseInCubic(float x)
        {
            return x * x * x;
        }

        /// <summary>
        /// Returns the amount of active projectiles in this world
        /// </summary>
        /// <returns></returns>
        public static int GetProjectileAmount()
        {
            int amount = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active)
                {
                    amount++;
                }
            }
            return amount;
        }

        /// <summary>
        /// Finds said item type in the player inventory.
        /// Returns null if not found
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Item FindItemInInventory(this Player player, int type, int minStack)
        {
            Item Item = null;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item p = player.inventory[i];
                if (p.stack < minStack)
                    continue;

                if (p != null && !p.IsAir && p.active && p.type == type)
                {
                    Item = p;
                }
            }
            return Item;
        }

        /// <summary>
        /// Finds said item type in the player inventory.
        /// Returns null if not found
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Item FindItemInInventory(this Player player, int type)
        {
            Item Item = null;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item p = player.inventory[i];
                if (p != null && !p.IsAir && p.active && p.type == type)
                {
                    Item = p;
                }
            }
            return Item;
        }

        /// <summary>
        /// Find and return the projectile index of this projectile type. Needs a player so owner check can be made.
        /// </summary>
        /// <param name="player">Owner</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int FindProjectileIndex(Player player, int type)
        {
            int proj = -1;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == player.whoAmI && p.type == type)
                {
                    proj = p.identity;
                }
            }
            return proj;
        }
    }
}