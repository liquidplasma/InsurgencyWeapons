using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    internal class InsurgencyGlobalItem : GlobalItem
    {
        private static SoundStyle AmmoNoise => new("InsurgencyWeapons/Sounds/Weapons/Craft/ammo")
        {
            MaxInstances = 0
        };

        private static SoundStyle GetWeapon => new("InsurgencyWeapons/Sounds/Weapons/Craft/gunpickup")
        {
            MaxInstances = 0
        };

        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (Insurgency.AmmoTypes.Contains(item.type))
            {
                Texture2D texture = TextureAssets.Item[item.type].Value;
                Rectangle rect = texture.Bounds;
                scale = 0.25f;
                ExtensionMethods.BetterEntityDraw(texture, item.Bottom + new Vector2(0, -4f), rect, lightColor, rotation, texture.Size() / 2, scale, SpriteEffects.None);
                return false;
            }
            return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Insurgency.AllWeapons.Contains(item.type))
            {
                int damage = (int)(Main.LocalPlayer.GetTotalDamage(DamageClass.Ranged).ApplyTo(item.damage) * Insurgency.WeaponScaling());
                int index = tooltips.FindIndex(tip => tip.Name == "Damage");
                tooltips.RemoveAt(index);
                TooltipLine actualDamage = new(Mod, "actualDamage", damage + Language.GetTextValue("LegacyTooltip.3"));
                tooltips.Insert(index, actualDamage);
            }
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