﻿using InsurgencyWeapons.Helpers;

namespace InsurgencyWeapons.Items.Ammo
{
    /// <summary>
    /// MP7 4.6x30mm Ammo
    /// </summary>
    internal class Bullet4630 : AmmoItem
    {
        public override void SetDefaults()
        {
            Money = 45;
            CraftStack = 40;
            Item.width = 7;
            Item.height = 24;
            Item.DefaultsToInsurgencyAmmo(11);
            base.SetDefaults();
        }
    }
}