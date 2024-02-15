using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    internal class InsurgencyGlobalItem : GlobalItem
    {
        public static SoundStyle AmmoNoise => new("InsurgencyWeapons/Sounds/Weapons/Craft/ammo")
        {
            MaxInstances = 0
        };

        public static SoundStyle GetWeapon => new("InsurgencyWeapons/Sounds/Weapons/Craft/gunpickup")
        {
            MaxInstances = 0
        };

        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (Insurgency.AmmoTypes.Contains(item.type))
            {
                Texture2D texture = item.MyTexture();
                Rectangle rect = texture.Bounds;
                scale = 0.25f;
                ExtensionMethods.BetterEntityDraw(texture, item.Bottom + new Vector2(0, -4f), rect, lightColor, rotation, texture.Size() / 2, scale, SpriteEffects.None);
                return false;
            }
            return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (InsurgencyModConfig.Instance.DamageScaling && Insurgency.AllWeapons.Contains(item.type))
            {
                damage *= Insurgency.WeaponScaling();
            }
            base.ModifyWeaponDamage(item, player, ref damage);
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is RecipeItemCreationContext)
            {
                if (Insurgency.AmmoTypes.Contains(item.type) ||
                    Insurgency.Grenades.Contains(item.type))
                {
                    SoundEngine.PlaySound(AmmoNoise, Main.LocalPlayer.Center);
                }

                if (Insurgency.AllWeapons.Contains(item.type))
                {
                    SoundEngine.PlaySound(GetWeapon, Main.LocalPlayer.Center);
                }
            }
        }
    }
}