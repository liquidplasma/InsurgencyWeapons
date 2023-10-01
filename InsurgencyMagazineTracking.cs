using InsurgencyWeapons.Items.Ammo;
using System;
using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    internal class InsurgencyMagazineTracking : ModPlayer
    {
        public bool isActive;

        //Assault rifles
        public int
            AKMMagazine,
            AN94Magazine,
            STGMagazine,
            AKS74UMagazine;

        //Battle rifles
        public int
            SCARHMagazine;

        //Revolvers
        public int
            PythonCylinder;

        //Rifles
        public int
            GarandMagazine,
            EnfieldMagazine;

        //Sniper Rifles
        public int
            M40A1Box,
            MosinBox;

        //Shotguns
        public int
            CoachBarrel,
            IthacaTube;

        //Sub machine guns
        public int
            MP7Magazine,
            M1928Drum,
            PPShDrum;

        //Light machine guns
        public int
            RPKDrum;

        public override void ResetEffects()
        {
            isActive = false;
        }

        /*private void UpdateMagazines()
        {
            //Assault rifles
            AKMMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet762>()), 0, 30);
            STGMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet792>()), 0, 30);
            AKS74UMagazine = AN94Magazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet545>()), 0, 30);

            //Battle rifles
            SCARHMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76251>()), 0, 20);

            //Revolvers
            PythonCylinder = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet357>()), 0, 6);

            //Rifles
            GarandMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet3006>()), 0, 8);
            EnfieldMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76251>()), 0, 10);

            //Sniper rifles
            M40A1Box = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76251>()), 0, 5);
            MosinBox = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76254R>()), 0, 5);

            //Shotguns
            CoachBarrel = Math.Clamp(Player.CountItem(ModContent.ItemType<ShellBuck_Ball>()), 0, 2);

            //Sub machine guns
            MP7Magazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet4630>()), 0, 40);
            M1928Drum = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet45ACP>()), 0, 50);
            PPShDrum = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76225>()), 0, 71);

            //Light machine guns
            RPKDrum = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet762>()), 0, 75);
        }*/

        public override void OnEnterWorld()
        {
            //UpdateMagazines();
        }
    }
}