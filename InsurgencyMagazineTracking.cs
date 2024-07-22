using InsurgencyWeapons.Helpers;
using Terraria.ModLoader.IO;

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
            foreach (NPC target in Main.ActiveNPCs)
            {
                if (target.friendly && target.Hitbox.Contains(Main.MouseWorld.ToPoint()))
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

        public override void SaveData(TagCompound tag)
        {
            //Assault rifles
            tag[nameof(AKMMagazine)] = AKMMagazine;
            tag[nameof(AN94Magazine)] = AN94Magazine;
            tag[nameof(ASValMagazine)] = ASValMagazine;
            tag[nameof(GalilMagazine)] = GalilMagazine;
            tag[nameof(GrozaMagazine)] = GrozaMagazine;
            tag[nameof(STGMagazine)] = STGMagazine;

            //Carbines
            tag[nameof(AKS74UMagazine)] = AKS74UMagazine;
            tag[nameof(G36CMagazine)] = G36CMagazine;
            tag[nameof(M1A1Magazine)] = M1A1Magazine;
            tag[nameof(M4A1Magazine)] = M4A1Magazine;
            tag[nameof(SKSMagazine)] = SKSMagazine;

            //Battle rifles
            tag[nameof(SCARHMagazine)] = SCARHMagazine;
            tag[nameof(G3A3Magazine)] = G3A3Magazine;

            //Pistols
            tag[nameof(M1911Magazine)] = M1911Magazine;

            //Revolvers
            tag[nameof(PythonCylinder)] = PythonCylinder;
            tag[nameof(M29Cylinder)] = M29Cylinder;

            //Rifles
            tag[nameof(GarandMagazine)] = GarandMagazine;
            tag[nameof(EnfieldMagazine)] = EnfieldMagazine;
            tag[nameof(SVTMagazine)] = SVTMagazine;

            //Sniper rifles
            tag[nameof(M40A1Box)] = M40A1Box;
            tag[nameof(MosinBox)] = MosinBox;

            //Shotguns
            tag[nameof(CoachBarrel)] = CoachBarrel;
            tag[nameof(IthacaTube)] = IthacaTube;
            tag[nameof(M590Tube)] = M590Tube;
            tag[nameof(M1014Tube)] = M1014Tube;

            //Sub machine guns
            tag[nameof(MP7Magazine)] = MP7Magazine;
            tag[nameof(MP5KMagazine)] = MP5KMagazine;
            tag[nameof(MP5SDMagazine)] = MP5SDMagazine;
            tag[nameof(M1928Drum)] = M1928Drum;
            tag[nameof(PPShDrum)] = PPShDrum;
            tag[nameof(UMP45Magazine)] = UMP45Magazine;

            //Light Machine guns
            tag[nameof(RPKDrum)] = RPKDrum;
            tag[nameof(M60Box)] = M60Box;
            tag[nameof(M249Box)] = M249Box;
            tag[nameof(PKMBox)] = PKMBox;
        }

        public override void LoadData(TagCompound tag)
        {
            //Assault rifles
            AKMMagazine = tag.GetInt(nameof(AKMMagazine));
            AN94Magazine = tag.GetInt(nameof(AN94Magazine));
            ASValMagazine = tag.GetInt(nameof(ASValMagazine));
            GalilMagazine = tag.GetInt(nameof(GalilMagazine));
            GrozaMagazine = tag.GetInt(nameof(GrozaMagazine));
            STGMagazine = tag.GetInt(nameof(STGMagazine));

            //Carbines
            AKS74UMagazine = tag.GetInt(nameof(AKS74UMagazine));
            G36CMagazine = tag.GetInt(nameof(G36CMagazine));
            M1A1Magazine = tag.GetInt(nameof(M1A1Magazine));
            M4A1Magazine = tag.GetInt(nameof(M4A1Magazine));
            SKSMagazine = tag.GetInt(nameof(SKSMagazine));

            //Battle rifles
            SCARHMagazine = tag.GetInt(nameof(SCARHMagazine));
            G3A3Magazine = tag.GetInt(nameof(G3A3Magazine));

            //Pistols
            M1911Magazine = tag.GetInt(nameof(M1911Magazine));

            //Revolvers
            PythonCylinder = tag.GetInt(nameof(PythonCylinder));
            M29Cylinder = tag.GetInt(nameof(M29Cylinder));
            GarandMagazine = tag.GetInt(nameof(GarandMagazine));
            EnfieldMagazine = tag.GetInt(nameof(EnfieldMagazine));
            SVTMagazine = tag.GetInt(nameof(SVTMagazine));

            //Rifles
            M40A1Box = tag.GetInt(nameof(M40A1Box));
            MosinBox = tag.GetInt(nameof(MosinBox));

            //Shotguns
            CoachBarrel = tag.GetInt(nameof(CoachBarrel));
            IthacaTube = tag.GetInt(nameof(IthacaTube));
            M590Tube = tag.GetInt(nameof(M590Tube));
            M1014Tube = tag.GetInt(nameof(M1014Tube));

            //Sub machine guns
            MP7Magazine = tag.GetInt(nameof(MP7Magazine));
            MP5KMagazine = tag.GetInt(nameof(MP5KMagazine));
            MP5SDMagazine = tag.GetInt(nameof(MP5SDMagazine));
            M1928Drum = tag.GetInt(nameof(M1928Drum));
            PPShDrum = tag.GetInt(nameof(PPShDrum));
            UMP45Magazine = tag.GetInt(nameof(UMP45Magazine));

            //Light Machine guns
            RPKDrum = tag.GetInt(nameof(RPKDrum));
            M60Box = tag.GetInt(nameof(M60Box));
            M249Box = tag.GetInt(nameof(M249Box));
            PKMBox = tag.GetInt(nameof(PKMBox));
        }
    }
}