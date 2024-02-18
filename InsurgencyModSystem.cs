using InsurgencyWeapons.Helpers;
using System.Collections.Generic;
using Terraria.UI;

namespace InsurgencyWeapons
{
    internal class InsurgencyModSystem : ModSystem
    {
        private InsurgencyAmmoStatsUI UI;
        private UserInterface AmmoDisplayUI;
        private Player Player => Main.LocalPlayer;
        private bool HoldingInsurgencyWeapon => Insurgency.AllWeapons.Contains(Player.HeldItem.type);

        public override void Load()
        {
            if (!Main.dedServ)
            {
                UI = new InsurgencyAmmoStatsUI();
                AmmoDisplayUI = new UserInterface();
                AmmoDisplayUI.SetState(UI);
            }
            base.Load();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            AmmoDisplayUI?.Update(gameTime);
            base.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
            if (HoldingInsurgencyWeapon && MouseIndex != -1)
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