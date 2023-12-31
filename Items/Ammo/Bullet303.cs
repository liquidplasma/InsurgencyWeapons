﻿using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// Lee-Enfield .303 Ammo
    /// </summary>
    internal class Bullet303 : AmmoItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 31;
            Item.DefaultsToInsurgencyAmmo(17);
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(money: 20, amountToCraft: 10);
        }
    }
}