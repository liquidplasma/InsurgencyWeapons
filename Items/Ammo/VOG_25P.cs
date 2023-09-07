using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Weapons.Ranged;
using InsurgencyWeapons.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// GP-25 40mm VOG-25P Grenade Ammo
    /// </summary>
    internal class VOG_25P : ModItem
    {
        private int
            realAKMDamage;

        private Item Ammo, AKM;

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 16;
            Item.DefaultsToInsurgencyAmmo(60);
        }

        public override void UpdateInventory(Player player)
        {
            if (player.HeldItem.type == ModContent.ItemType<AKM>())
            {
                int akmIndex = HelperStats.FindProjectileIndex(player, ModContent.ProjectileType<AKMHeld>());
                if (Main.projectile.IndexInRange(akmIndex))
                {
                    Projectile H = Main.projectile[akmIndex];
                    if (H.active)
                    {
                        AKMHeld AKMHeldProj = H.ModProjectile as AKMHeld;
                        realAKMDamage = AKMHeldProj.VOGDamage;
                        Ammo = AKMHeldProj.Ammo;
                    }
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AKM ??= ContentSamples.ItemsByType[ModContent.ItemType<AKM>()];
            if (AKM != null && Ammo != null && realAKMDamage > 0)
            {
                int index = tooltips.FindIndex(tip => tip.Name == "Damage");
                tooltips.RemoveAt(index);
                Main.NewText(realAKMDamage);
                TooltipLine actualDamage = new(Mod, "actualDamage", realAKMDamage + Language.GetTextValue("LegacyTooltip.3"));
                tooltips.Insert(index, actualDamage);
            }
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 30, amountToCraft: 1);
        }
    }
}