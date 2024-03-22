using InsurgencyWeapons.Items;
using InsurgencyWeapons.Items.Weapons.Revolvers;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;

namespace InsurgencyWeapons
{
    public class InsurgencyTooltipWeaponClass : GlobalItem
    {
        private static int[] DoubleActionRevolvers => new int[] { ModContent.ItemType<M29>(), ModContent.ItemType<ColtPython>() };

        public override bool InstancePerEntity => true;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string weaponClass = "";
            if (item.ModItem is not null and AssaultRifle)
                //Assault rifles
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.AR");

            if (item.ModItem is not null and BattleRifle)
                //Battle Rifles
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.BR");

            if (item.ModItem is not null and Carbine)
                //Carbines
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.CAR");

            if (item.ModItem is not null and Grenade)
                //Grenades
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.Gren");

            if (item.ModItem is not null and Revolver)
            {
                //Revolvers
                if (DoubleActionRevolvers.Contains(item.type))
                    weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.RevoDB");
                //if (item.type == ModContent.ItemType<Webley>())
                //weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.RevoBA");
            }

            if (item.ModItem is not null and Rifle)
                //Rifles
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.Rifle");

            if (item.ModItem is not null and SniperRifle)
                //Sniper rifles
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.Sniper");

            if (item.ModItem is not null and Shotgun)
                //Sniper rifles
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.Shotgun");

            if (item.ModItem is not null and Pistol)
                //Pistols
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.Pistol");

            if (item.ModItem is not null and SubMachineGun)
                //Sub machine guns
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.SMG");

            if (item.ModItem is not null and LightMachineGun)
                //Light machine guns
                weaponClass = Language.GetTextValue("Mods.InsurgencyWeapons.WeaponClasses.LMG");

            TooltipLine wclass = new(Mod, "wclass", weaponClass)
            {
                OverrideColor = Color.LightGreen
            };
            tooltips.Add(wclass);
        }
    }
}