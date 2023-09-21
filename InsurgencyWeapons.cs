using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    public class InsurgencyWeapons : Mod
    {
        public override void PostSetupContent()
        {
            for (int i = 1; i < ItemLoader.ItemCount; i++)
            {
                Item item = ContentSamples.ItemsByType[i];

                if (item.ModItem is not null and AmmoItem)
                    //Ammo types
                    Insurgency.AmmoTypes.Add(item.type);

                if (item.ModItem is not null and WeaponUtils and not Grenade)
                    //All weapons
                    Insurgency.AllWeapons.Add(item.type);

                if (item.ModItem is not null and AssaultRifle)
                    //Assault rifles
                    Insurgency.AssaultRifles.Add(item.type);

                if (item.ModItem is not null and Carbine)
                    //Carbines
                    Insurgency.Carbines.Add(item.type);

                if (item.ModItem is not null and Grenade)
                    //Grenades
                    Insurgency.Grenades.Add(item.type);

                if (item.ModItem is not null and Revolver)
                    //Revolvers
                    Insurgency.Revolvers.Add(item.type);

                if (item.ModItem is not null and Rifle)
                    //Rifles
                    Insurgency.Rifles.Add(item.type);

                if (item.ModItem is not null and SubMachineGun)
                    //Sub machine guns
                    Insurgency.SubMachineGuns.Add(item.type);
            }
        }

        public override void Unload()
        {
            Insurgency.AmmoTypes.Clear();
            Insurgency.AllWeapons.Clear();

            Insurgency.AssaultRifles.Clear();
            Insurgency.Carbines.Clear();
            Insurgency.Grenades.Clear();
            Insurgency.Rifles.Clear();
            Insurgency.Revolvers.Clear();
            Insurgency.SubMachineGuns.Clear();
        }
    }
}