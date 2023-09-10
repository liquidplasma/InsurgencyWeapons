using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Helpers
{
    internal static class ExtensionMethods
    {
        public static void DefaultsToInsurgencyAmmo(this Item Item, int damage)
        {
            Item.damage = damage;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = Item.type;
            Item.shoot = Insurgency.Bullet;
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
            Item.maxStack = 999;
        }

        public static void RegisterINS2RecipeAmmo(this ModItem Item, int money, int amountToCraft = 1)
        {
            Item.CreateRecipe(amountToCraft)
                .AddIngredient(Insurgency.Money, money)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public static void RegisterINS2RecipeWeapon(this ModItem Item, int money)
        {
            Item.CreateRecipe()
                .AddIngredient(ItemID.IllegalGunParts, 2)
                .AddIngredient(Insurgency.Money, money)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        /// <summary>
        /// Associated texture for this projectile type
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
        public static Texture2D MyTexture(this Projectile projectile) => TextureAssets.Projectile[projectile.type].Value;

        /// <summary>
        /// Projectile will bounce back, best use in OnTileCollide
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="radians">Radians rotated randomly when bounced</param>
        public static void Bounce(this Projectile proj, int radians = 0)
        {
            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(proj.velocity.X - proj.oldVelocity.X) > float.Epsilon)
            {
                proj.velocity.X = -proj.oldVelocity.X;
            }

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(proj.velocity.Y - proj.oldVelocity.Y) > float.Epsilon)
            {
                proj.velocity.Y = -proj.oldVelocity.Y;
            }
            proj.velocity = proj.velocity.RotatedByRandom(MathHelper.ToRadians(radians));
            proj.netUpdate = true;
        }

        /// <summary>
        /// This projectile will rotate based on it's velocity with the following formula
        /// <code>projectile.rotation += (projectile.velocity.X * 0.04f) + (projectile.velocity.Y * 0.04f)</code>
        /// </summary>
        /// <param name="projectile"></param>
        public static void RotateBasedOnVelocity(this Projectile projectile)
        {
            projectile.rotation += (projectile.velocity.X * 0.04f) + (projectile.velocity.Y * 0.04f);
        }

        /// <summary>
        /// Correctly angles a projectile sprite based on velocity if the sprite is facing up.
        /// <para>Rotating in the direction of travel is often used in projectiles like arrows.</para>
        /// </summary>
        /// <param name="proj"></param>
        public static void FaceForward(this Projectile proj)
        {
            proj.rotation = proj.velocity.ToRotation() + MathHelper.PiOver2;
        }

        /// <summary>
        /// Returns the player stealth
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static float GetStealth(this Player player)
        {
            if (player.shroomiteStealth)
                return Math.Clamp(Math.Abs(player.stealth - 2f), 0f, 1.6f);
            return Math.Clamp(Math.Abs(player.stealth - 2f), 0f, 1.8f);
        }

        /// <summary>
        /// Used on my held projectiles
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="Projectile"></param>
        public static void HoldOutArm(this Player Player, Projectile Projectile, Vector2 vector)
        {
            float VisualRotation = Projectile.Center.AngleTo(vector);
            if (Player.direction == 1)
            {
                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, VisualRotation - MathHelper.PiOver2 + (MathHelper.PiOver2 / (10 - Math.Abs(VisualRotation))));
            }
            else
            {
                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, VisualRotation - MathHelper.PiOver2 - (MathHelper.PiOver2 / (10 - Math.Abs(VisualRotation))));
            }
        }

        /// <summary> Checks if the player that owns this projectile is alive, else kill this projectile. </summary>>
        public static void CheckPlayerActiveAndNotDead(this Projectile proj, Player owner)
        {
            if (owner.dead || !owner.active)
                proj.Kill();
            if (!owner.dead || owner.active)
                proj.timeLeft = 2;
        }
    }
}