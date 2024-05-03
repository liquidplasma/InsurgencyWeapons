using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using InsurgencyWeapons.Items.Ammo;

namespace InsurgencyWeapons
{
    public class InsurgencyMagazineTracking : ModPlayer
    {
        public bool MouseOverFriendlyNPC { get; set; }

        public bool
            isActive;

        //Assault rifles
        public int
            AKMMagazine,
            AN94Magazine,
            ASValMagazine,
            GalilMagazine,
            GrozaMagazine,
            STGMagazine;

        //Carbines
        public int
            AKS74UMagazine,
            G36CMagazine,
            M1A1Magazine,
            M4A1Magazine,
            SKSMagazine;

        //Battle rifles
        public int
            SCARHMagazine,
            G3A3Magazine;

        //Handguns
        public int
            M1911Magazine;

        //Revolvers
        public int
            PythonCylinder,
            M29Cylinder;

        //Rifles
        public int
            GarandMagazine,
            EnfieldMagazine,
            SVTMagazine;

        //Sniper Rifles
        public int
            M40A1Box,
            MosinBox;

        //Shotguns
        public int
            CoachBarrel,
            IthacaTube,
            M590Tube,
            M1014Tube;

        //Sub machine guns
        public int
            MP7Magazine,
            MP5KMagazine,
            MP5SDMagazine,
            M1928Drum,
            PPShDrum,
            UMP45Magazine;

        //Light machine guns
        public int
            RPKDrum,
            M60Box,
            M249Box,
            PKMBox;

        //Ammo display UI
        public int
            CurrentAmmo,
            AmmoType,
            AmmoTypeGL;

        public bool HasGL;

        public string GrenadeName;

        public void BuildUI(int currentAmmo, int ammoType, bool hasGL, int ammoTypeGL, string grenadeName)
        {
            CurrentAmmo = currentAmmo;
            AmmoType = ammoType;
            HasGL = hasGL;
            AmmoTypeGL = ammoTypeGL;
            GrenadeName = grenadeName;
        }

        public override void ResetEffects()
        {
            isActive = false;
        }

        private bool OverFriendlyNPC()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (target.active && target.friendly && target.Hitbox.Contains(Main.MouseWorld.ToPoint()))
                    return true;
            }
            return false;
        }

        public override void PostUpdate()
        {
            if (Player.HoldingInsurgencyWeapon() && isActive)
                MouseOverFriendlyNPC = OverFriendlyNPC();
            base.PostUpdate();
        }

        private void UpdateMagazines()
        {
            //Assault rifles
            AKMMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet762>()), 0, 30);
            STGMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet792>()), 0, 30);
            AN94Magazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet545>()), 0, 30);
            ASValMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet939>()), 0, 20);
            GalilMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet556>()), 0, 35);
            GrozaMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet939>()), 0, 20);

            //Battle rifles
            SCARHMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76251>()), 0, 20);
            G3A3Magazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76251>()), 0, 20);

            //Carbines
            AKS74UMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet545>()), 0, 30);
            G36CMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet556>()), 0, 30);
            M1A1Magazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76233>()), 0, 15);
            M4A1Magazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet556>()), 0, 30);
            SKSMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet762>()), 0, 20);

            //Handguns
            M1911Magazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet45ACP>()), 0, 7);

            //Revolvers
            PythonCylinder = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet357>()), 0, 6);
            M29Cylinder = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet357>()), 0, 6);

            //Rifles
            GarandMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet3006>()), 0, 8);
            EnfieldMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76251>()), 0, 10);

            //Sniper rifles
            M40A1Box = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76251>()), 0, 5);
            MosinBox = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76254R>()), 0, 5);

            //Shotguns
            CoachBarrel = Math.Clamp(Player.CountItem(ModContent.ItemType<ShellBuck_Ball>()), 0, 2);
            IthacaTube = Math.Clamp(Player.CountItem(ModContent.ItemType<TwelveGauge>()), 0, 6);
            M590Tube = Math.Clamp(Player.CountItem(ModContent.ItemType<TwelveGauge>()), 0, 8);
            M1014Tube = Math.Clamp(Player.CountItem(ModContent.ItemType<TwelveGauge>()), 0, 7);

            //Sub machine guns
            MP7Magazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet4630>()), 0, 40);
            MP5KMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet9x19>()), 0, 30);
            MP5SDMagazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet9x19>()), 0, 30);
            M1928Drum = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet45ACP>()), 0, 50);
            PPShDrum = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76225>()), 0, 71);
            UMP45Magazine = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet45ACP>()), 0, 25);

            //Light machine guns
            RPKDrum = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet762>()), 0, 75);
            M60Box = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76251>()), 0, 100);
            M249Box = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet556>()), 0, 200);
            PKMBox = Math.Clamp(Player.CountItem(ModContent.ItemType<Bullet76254R>()), 0, 100);
        }

        public override void OnEnterWorld()
        {
            UpdateMagazines();
        }

        public override void OnRespawn()
        {
            UpdateMagazines();
        }
    }
}