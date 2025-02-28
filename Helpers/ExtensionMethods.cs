﻿using InsurgencyWeapons.VendingMachines.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace InsurgencyWeapons.Helpers
{
    public static class ExtensionMethods
    {
        public static void Animate(this Projectile proj, int tickPerFrame)
        {
            if (++proj.frameCounter >= tickPerFrame)
            {
                proj.frameCounter = 0;
                proj.frame = ++proj.frame % Main.projFrames[proj.type];
            }
        }

        public static void ConsumeMultiple(this Player player, int amountToConsume, int type)
        {
            if (amountToConsume == 1)
            {
                player.ConsumeItem(type);
                return;
            }
            for (int i = 0; i < amountToConsume; i++)
            {
                player.ConsumeItem(type);
            }
        }

        public static CombatText CreateCombatText(Player sourcePlayer, Color color, string text) => Main.combatText[CombatText.NewText(sourcePlayer.getRect(), color, text)];

        public static CombatText CreateCombatText(NPC sourceNPC, Color color, string text) => Main.combatText[CombatText.NewText(sourceNPC.getRect(), color, text)];

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
            Item.maxStack = Item.CommonMaxStack;
        }

        public static void RegisterINS2RecipeAmmo(this ModItem Item, int money, int amountToCraft = 1)
        {
            Item.CreateRecipe(amountToCraft)
                .AddIngredient(Insurgency.Money, money)
                .AddTile(ModContent.TileType<VendingMachineAmmoTile>())
                .Register();
        }

        public static void RegisterINS2RecipeWeapon(this ModItem Item, int money)
        {
            Item.CreateRecipe()
                .AddIngredient(Insurgency.Money, money)
                .AddTile(ModContent.TileType<VendingMachineGunsTile>())
                .Register();
        }

        public static void RegisterINS2RecipeLauncher(this ModItem Item, int money, int amountToCraft = 1)
        {
            Item.CreateRecipe(amountToCraft)
                .AddIngredient(Insurgency.Money, money)
                .AddTile(ModContent.TileType<VendingMachineGunsTile>())
                .Register();
        }

        /// <summary>
        /// Associated texture for this item type
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
        public static Texture2D MyTexture(this Item Item) => TextureAssets.Item[Item.type].Value;

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
        public static void RotateBasedOnVelocity(this Projectile projectile, float mult = 0.04f) => projectile.rotation += (projectile.velocity.X * mult);

        /// <summary>
        /// Correctly angles a projectile sprite based on velocity if the sprite is facing up.
        /// <para>Rotating in the direction of travel is often used in projectiles like arrows.</para>
        /// </summary>
        /// <param name="proj"></param>
        public static void FaceForward(this Projectile proj) => proj.rotation = proj.velocity.ToRotation() + MathHelper.PiOver2;

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
        public static void HoldOutArm(this Player Player, Projectile Projectile, Vector2 angleVector)
        {
            //Vanilla code below, ech
            float angleFloat = MathF.Sin(Player.Center.AngleTo(angleVector));
            bool addAngle = HelperStats.TestRange(angleFloat, 0.12f, 1f);
            angleFloat /= 2f;
            float num7 = -MathF.PI / 10f;
            if (Player.direction == -1)
            {
                num7 *= -1f;
            }
            float num8 = Projectile.rotation - MathF.PI / 4f + MathF.PI;
            if (Player.direction == 1)
            {
                num8 += MathF.PI / 2f;
            }
            float rotation = num8 + num7;
            if (Player.direction == 1)
            {
                if (addAngle)
                    rotation -= angleFloat;
                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
            }
            else
            {
                if (addAngle)
                    rotation += angleFloat;
                Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
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