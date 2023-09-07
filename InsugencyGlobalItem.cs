using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    internal class InsugencyGlobalItem : GlobalItem
    {
        private static SoundStyle CraftNoise => new("InsurgencyWeapons/Sounds/Weapons/Ins2/Craft/ammo")
        {
            MaxInstances = 0
        };

        private static SoundStyle GetWeapon => new("InsurgencyWeapons/Sounds/Weapons/Ins2/Craft/gunpickup")
        {
            MaxInstances = 0
        };

        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (Insurgency.AmmoTypes.Contains(item.type))
            {                
                Texture2D texture = TextureAssets.Item[item.type].Value;
                Rectangle rect = texture.Bounds;
                scale = 0.5f;
                Main.EntitySpriteDraw(texture, item.Center - Main.screenPosition, rect, Color.White, rotation, texture.Size() / 2, scale, SpriteEffects.None);
                return false;
            }
            return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is RecipeItemCreationContext)
            {
                if (Insurgency.AmmoTypes.Contains(item.type))
                    SoundEngine.PlaySound(CraftNoise, Main.LocalPlayer.Center);

                if (Insurgency.WeaponTypes.Contains(item.type))
                    SoundEngine.PlaySound(GetWeapon, Main.LocalPlayer.Center);
            }
        }
    }
}