using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.AssaultRifles;
using InsurgencyWeapons.Items.Weapons.Rifles;
using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    public class InsurgencyWeapons : Mod
    {
        public override void PostSetupContent()
        {
            //Ammo types
            Insurgency.AmmoTypes.Add(ModContent.ItemType<Bullet762>());
            Insurgency.AmmoTypes.Add(ModContent.ItemType<VOG_25P>());
            Insurgency.AmmoTypes.Add(ModContent.ItemType<Bullet3006>());
            Insurgency.AmmoTypes.Add(ModContent.ItemType<Bullet792>());

            //Weapon types
            Insurgency.AllWeapons.Add(ModContent.ItemType<AKM>());
            Insurgency.AllWeapons.Add(ModContent.ItemType<STG44>());
            //Insurgency.AllWeapons.Add(ModContent.ItemType<M1Garand>());

            //Assault rifles
            Insurgency.AssaultRifles.Add(ModContent.ItemType<AKM>());
            Insurgency.AssaultRifles.Add(ModContent.ItemType<STG44>());

            //Rifles
            //Insurgency.Rifles.Add(ModContent.ItemType<M1Garand>());
        }

        public override void Unload()
        {
            Insurgency.AmmoTypes.Clear();
            Insurgency.AllWeapons.Clear();

            Insurgency.AssaultRifles.Clear();
            Insurgency.Rifles.Clear();
        }
    }
}