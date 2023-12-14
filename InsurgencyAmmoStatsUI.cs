using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace InsurgencyWeapons
{
    internal class InsurgencyAmmoStatsUI : UIState
    {
        private UIText AmmoDisplay;
        private UIElement Area;
        private Player Player => Main.LocalPlayer;
        private InsurgencyMagazineTracking AmmoTracking => Player.GetModPlayer<InsurgencyMagazineTracking>();
        private bool HoldingInsurgencyWeapon => Insurgency.AllWeapons.Contains(Player.HeldItem.type);
        private bool OverFriendlyNPC => AmmoTracking.mouseOverFriendlyNPC;

        public override void OnInitialize()
        {
            Area = new UIElement();
            Area.Left.Set(Main.MouseWorld.Y, 0f);
            Area.Top.Set(Main.MouseWorld.X, 0f);
            Recalculate();
            Area.Width.Set(182, 0f);
            Area.Height.Set(60, 0f);
            AmmoDisplay = new UIText("");
            AmmoDisplay.Width.Set(138, 0f);
            AmmoDisplay.Height.Set(34, 0f);
            AmmoDisplay.Top.Set(40, 0f);
            AmmoDisplay.Left.Set(0, 0f);
            Area.Append(AmmoDisplay);
            Append(Area);
            base.OnInitialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            bool overGrave = Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType == 85;
            if (!HoldingInsurgencyWeapon || !AmmoTracking.isActive || overGrave || Player.mouseInterface || OverFriendlyNPC)
                return;
            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Area.Left.Set(Main.mouseX - 35, 0f);
            Area.Top.Set(Main.mouseY, 0f);
            Recalculate();
            base.DrawSelf(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            bool overGrave = Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType == 85;
            if (!HoldingInsurgencyWeapon || !AmmoTracking.isActive || overGrave || Player.mouseInterface || OverFriendlyNPC)
                return;

            string IconAmmo;
            Item Ammo = ContentSamples.ItemsByType[AmmoTracking.AmmoType];
            if (Ammo != null && AmmoTracking.HasGL && AmmoTracking.AmmoTypeGL != -1)
            {
                Item AmmoGL = ContentSamples.ItemsByType[AmmoTracking.AmmoTypeGL];
                IconAmmo = $"[i:InsurgencyWeapons/{Ammo.ModItem?.Name}]";
                string IconGrenade;
                IconGrenade = $"[i:InsurgencyWeapons/{AmmoGL.ModItem?.Name}]";
                AmmoDisplay.SetText(AmmoTracking.CurrentAmmo + " / " + Player.CountItem(AmmoTracking.AmmoType) + IconAmmo + AmmoTracking.GrenadeName + Player.CountItem(AmmoTracking.AmmoTypeGL) + IconGrenade);
            }
            else if (Ammo != null)
            {
                IconAmmo = $"[i:InsurgencyWeapons/{Ammo.ModItem?.Name}]";
                AmmoDisplay.SetText(AmmoTracking.CurrentAmmo + " / " + Player.CountItem(AmmoTracking.AmmoType) + IconAmmo);
            }
            base.Update(gameTime);
        }
    }
}