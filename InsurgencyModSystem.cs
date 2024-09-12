using InsurgencyWeapons.Helpers;
using System.Collections.Generic;
using Terraria.UI;

namespace InsurgencyWeapons
{
    public class InsurgencyModSystem : ModSystem
    {
        public static InsurgencyModSystem Instance { get; set; }
        public int AmmoSellerSpawnDelay;

        private InsurgencyAmmoStatsUI UI;

        private UserInterface AmmoDisplayUI;

        private Player Player => Main.LocalPlayer;
        private InsurgencyMagazineTracking AmmoTracking => Player.GetModPlayer<InsurgencyMagazineTracking>();
        private bool OverFriendlyNPC => AmmoTracking.MouseOverFriendlyNPC;

        private bool HoldingInsurgencyWeapon => Insurgency.AllWeapons.Contains(Player.HeldItem.type);

        public override void Load()
        {
            Instance = this;
            if (!Main.dedServ)
            {
                UI = new InsurgencyAmmoStatsUI();
                AmmoDisplayUI = new UserInterface();
                AmmoDisplayUI.SetState(UI);
            }
            base.Load();
        }

        public override void PostUpdateNPCs()
        {
            if (AmmoSellerSpawnDelay > 0)
                AmmoSellerSpawnDelay--;

            base.PostUpdateNPCs();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            AmmoDisplayUI?.Update(gameTime);
            base.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            bool overGrave = Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType == TileID.Tombstones;
            bool candraw = HoldingInsurgencyWeapon && AmmoTracking.isActive && !overGrave && !Player.mouseInterface && !OverFriendlyNPC;
            int MouseIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
            if (candraw && MouseIndex != -1)
            {
                layers.Insert(MouseIndex, new LegacyGameInterfaceLayer(
                    "AmmmoDisplay",
                    delegate
                    {
                        AmmoDisplayUI.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            base.ModifyInterfaceLayers(layers);
        }
    }
}