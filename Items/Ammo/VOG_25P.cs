using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Weapons.AssaultRifles;
using InsurgencyWeapons.Projectiles.AssaultRifles;
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
            realVOGDamage;

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
                        realVOGDamage = AKMHeldProj.VOGDamage;
                        Ammo = AKMHeldProj.Ammo;
                    }
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AKM ??= ContentSamples.ItemsByType[ModContent.ItemType<AKM>()];
            if (AKM != null && Ammo != null && realVOGDamage > 0)
            {
                int index = tooltips.FindIndex(tip => tip.Name == "Damage");
                tooltips.RemoveAt(index);
                TooltipLine actualDamage = new(Mod, "actualDamage", realVOGDamage + Language.GetTextValue("LegacyTooltip.3"));
                tooltips.Insert(index, actualDamage);
            }
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 30, amountToCraft: 1);
        }
    }
}