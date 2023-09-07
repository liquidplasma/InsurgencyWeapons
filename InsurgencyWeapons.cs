using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Items.Weapons.Ranged;
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
            Insurgency.WeaponTypes.Add(ModContent.ItemType<AKM>());
            Insurgency.WeaponTypes.Add(ModContent.ItemType<STG44>());
            Insurgency.WeaponTypes.Add(ModContent.ItemType<M1Garand>());

            //Assault rifles
            Insurgency.AssaultRifles.Add(ModContent.ItemType<AKM>());
            Insurgency.AssaultRifles.Add(ModContent.ItemType<STG44>());

            //Rifles
            Insurgency.Rifles.Add(ModContent.ItemType<M1Garand>());
        }

        public override void Unload()
        {
            Insurgency.AmmoTypes.Clear();
            Insurgency.WeaponTypes.Clear();
        }
    }
}