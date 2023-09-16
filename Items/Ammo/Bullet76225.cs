using InsurgencyWeapons.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// PPSh-41 7.62x25mm Ammo
    /// </summary>
    internal class Bullet76225 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 17;
            Item.DefaultsToInsurgencyAmmo(9);
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 50, amountToCraft: 71);
        }
    }
}