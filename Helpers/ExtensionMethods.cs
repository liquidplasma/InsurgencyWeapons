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
        /// <summary>
        /// This EntityDraw call already subtracts screen position
        /// <code>position - Main.screenPosition</code>
        /// </summary>
        public static void BetterEntityDraw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float worthless = 0f)
        {
            Main.EntitySpriteDraw(texture, position - Main.screenPosition, sourceRectangle, color, rotation, origin, new Vector2(scale), effects, worthless);
        }

        public static void DefaultsToInsurgencyAmmo(this Item Item, int damage)
        {
            Item.damage = damage;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = Item.type;
            Item.shoot = Insurgency.Bullet;
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
            Item.maxStack = Item.CommonMaxStack;
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
        /// <code>projectile.rotation += (projectile.velocity.X * mult)</code>
        /// </summary>
        /// <param name="projectile"></param>
        public static void RotateBasedOnVelocity(this Projectile projectile, float mult = 0.04f)
        {
            projectile.rotation += (projectile.velocity.X * mult);
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
        public static void HoldOutArm(this Player Player, Vector2 vector, float offset)
        {
            float VisualRotation = Player.MountedCenter.AngleTo(vector + new Vector2(offset));
            float sin = (float)Math.Sin(VisualRotation) * 0.5f;            
            if (Player.direction == 1)
            {
                VisualRotation -= sin;
                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.ThreeQuarters, VisualRotation - MathHelper.PiOver2 + (MathHelper.PiOver2 / (10 - Math.Abs(VisualRotation))));
            }
            else
            {
                VisualRotation += sin;
                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.ThreeQuarters, VisualRotation - MathHelper.PiOver2 - (MathHelper.PiOver2 / (10 - Math.Abs(VisualRotation))));
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