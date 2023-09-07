using InsurgencyWeapons.Items.Ammo;
using System;
using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    internal class InsurgencyMagazineTracking : ModPlayer
    {
        public bool isActive;

        public int
            AKMMagazine,
            AKMVOG_25P,
            STGMagazine,
            GarandMagazine;

        public override void ResetEffects()
        {
            isActive = false;
        }

        public override void OnEnterWorld()
        {
            //Assault rifles
            AKMMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet762>()), 0, 30);
            AKMVOG_25P = Math.Clamp(Player.CountItem(ModContent.ItemType<VOG_25P>()), 1, 1);
            STGMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet792>()), 0, 30);

            //Rifles
            GarandMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet3006>()), 0, 8);
        }
    }
}