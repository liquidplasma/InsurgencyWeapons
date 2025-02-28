using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using InsurgencyWeapons.Items.Other;
using Terraria.GameContent.UI;

namespace InsurgencyWeapons
{
    public class InsurgencyWeapons : Mod
    {
        public static Mod Instance => ModContent.GetInstance<InsurgencyWeapons>();

        public static int MoneyCurrency;

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

                if (item.ModItem is not null and BattleRifle)
                    //Battle rifles
                    Insurgency.BattleRifles.Add(item.type);

                if (item.ModItem is not null and Carbine)
                    //Carbines
                    Insurgency.Carbines.Add(item.type);

                if (item.ModItem is not null and Launcher)
                    //Launchers
                    Insurgency.Launchers.Add(item.type);

                if (item.ModItem is not null and Grenade)
                    //Grenades
                    Insurgency.Grenades.Add(item.type);

                if (item.ModItem is not null and Revolver)
                    //Revolvers
                    Insurgency.Revolvers.Add(item.type);

                if (item.ModItem is not null and Rifle)
                    //Rifles
                    Insurgency.Rifles.Add(item.type);

                if (item.ModItem is not null and Pistol)
                    //Pistols
                    Insurgency.Pistols.Add(item.type);

                if (item.ModItem is not null and SniperRifle)
                    //Sniper rifles
                    Insurgency.SniperRifles.Add(item.type);

                if (item.ModItem is not null and Shotgun)
                    //Shotguns
                    Insurgency.Shotguns.Add(item.type);

                if (item.ModItem is not null and SubMachineGun)
                    //Sub machine guns
                    Insurgency.SubMachineGuns.Add(item.type);

                if (item.ModItem is not null and LightMachineGun)
                    //Light machine guns
                    Insurgency.LightMachineGuns.Add(item.type);
            }
        }

        public override void Load()
        {
            On_Main.DrawCursor += On_Main_DrawCursor;
            On_Main.DrawThickCursor += On_Main_DrawThickCursor;
            MoneyCurrency = CustomCurrencyManager.RegisterCurrency(new Currency.MoneyForShop(ModContent.ItemType<Money>(), 9999L, "Mods.InsurgencyWeapons.Items.Money.DisplayName"));
            base.Load();
        }

        private Vector2 On_Main_DrawThickCursor(On_Main.orig_DrawThickCursor orig, bool smart)
        {
            if (InsurgencyModConfigClient.Instance.ShowCrosshair && !Main.LocalPlayer.mouseInterface && Main.LocalPlayer.TryGetModPlayer(out InsurgencyMagazineTracking result) && result.DrawingCrosshair)
                return Vector2.Zero;

            return orig(smart);
        }

        private void On_Main_DrawCursor(On_Main.orig_DrawCursor orig, Vector2 bonus, bool smart)
        {
            if (InsurgencyModConfigClient.Instance.ShowCrosshair && !Main.LocalPlayer.mouseInterface && Main.LocalPlayer.TryGetModPlayer(out InsurgencyMagazineTracking result) && result.DrawingCrosshair)
                return;

            orig(bonus, smart);
        }

        public override void Unload()
        {
            Insurgency.AmmoTypes.Clear();
            Insurgency.AllWeapons.Clear();

            Insurgency.AssaultRifles.Clear();
            Insurgency.Carbines.Clear();
            Insurgency.Grenades.Clear();
            Insurgency.Rifles.Clear();
            Insurgency.Shotguns.Clear();
            Insurgency.SniperRifles.Clear();
            Insurgency.Revolvers.Clear();
            Insurgency.SubMachineGuns.Clear();
            Insurgency.LightMachineGuns.Clear();
        }
    }
}