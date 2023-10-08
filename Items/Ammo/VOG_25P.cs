using InsurgencyWeapons.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// GP-25 40mm VOG-25P Grenade Ammo
    /// </summary>
    internal class VOG_25P : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 6;
            Item.height = 18;
            Item.DefaultsToInsurgencyAmmo(60);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(tip => tip.Name == "Damage");
            tooltips.RemoveAt(index);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 30, amountToCraft: 1);
        }
    }
}