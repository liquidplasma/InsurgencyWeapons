using InsurgencyWeapons.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// M1 Garand 7.62x63mm Ammo
    /// </summary>
    internal class Bullet3006 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 36;
            Item.DefaultsToInsurgencyAmmo(14);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 15, amountToCraft: 8);
        }
    }
}