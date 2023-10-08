using InsurgencyWeapons.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Webley .455 WMk.II
    /// </summary>
    internal class Bullet455 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 18;
            Item.DefaultsToInsurgencyAmmo(13);
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 10, amountToCraft: 6);
        }
    }
}