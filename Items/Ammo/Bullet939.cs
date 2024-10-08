﻿using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// 9x39mm Ammo
    /// </summary>
    public class Bullet939 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 20;
            CraftStack = 20;
            Item.width = 7;
            Item.height = 26;
            Item.DefaultsToInsurgencyAmmo(12);
            base.SetDefaults();
        }
    }
}