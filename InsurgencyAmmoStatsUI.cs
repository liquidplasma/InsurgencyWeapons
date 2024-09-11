using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using InsurgencyWeapons.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace InsurgencyWeapons
{
    public class InsurgencyAmmoStatsUI : UIState
    {
        private UIText AmmoDisplayText;

        private UIElement AmmoArea;

        private Player Player => Main.LocalPlayer;
        private float CrosshairSpread = 1f;

        private Texture2D Crosshair = ModContent.Request<Texture2D>("InsurgencyWeapons/Textures/Crosshair", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        private Texture2D CrosshairBorder = ModContent.Request<Texture2D>("InsurgencyWeapons/Textures/CrosshairBorder", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        private InsurgencyMagazineTracking AmmoTracking => Player.GetModPlayer<InsurgencyMagazineTracking>();
        private bool HoldingInsurgencyWeapon => Player.HoldingInsurgencyWeapon();
        private bool OverFriendlyNPC => AmmoTracking.MouseOverFriendlyNPC;

        public override void OnInitialize()
        {
            AmmoArea = new UIElement();
            AmmoArea.Left.Set(Main.MouseWorld.Y, 0f);
            AmmoArea.Top.Set(Main.MouseWorld.X, 0f);
            Recalculate();
            AmmoArea.Width.Set(182, 0f);
            AmmoArea.Height.Set(60, 0f);
            AmmoDisplayText = new UIText("");
            AmmoDisplayText.Width.Set(138, 0f);
            AmmoDisplayText.Height.Set(34, 0f);
            AmmoDisplayText.Top.Set(40, 0f);
            AmmoDisplayText.Left.Set(0, 0f);
            AmmoArea.Append(AmmoDisplayText);
            Append(AmmoArea);
            base.OnInitialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            bool overGrave = Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType == TileID.Tombstones;
            if (!HoldingInsurgencyWeapon || !AmmoTracking.isActive || overGrave || Player.mouseInterface || OverFriendlyNPC)
                return;
            Item Item = Player.HeldItem;
            if (Item.ModItem is WeaponUtils GrabProj)
            {
                int insurgencyWeaponProjType = GrabProj.WeaponHeldProjectile;
                Projectile InsurgencyWeaponProj = Main.projectile[HelperStats.FindProjectileIndex(Player, insurgencyWeaponProjType)];
                if (InsurgencyWeaponProj.active && InsurgencyWeaponProj.ModProjectile is WeaponBase GetStats)
                {
                    if (GetStats.Degree != 0 && CrosshairSpread <= 2f)
                        CrosshairSpread += GetStats.Degree * 0.25f;
                    Main.cursorColor = Color.Transparent;
                    Main.MouseBorderColor = Color.Transparent;

                    float offsetDistance = 15f * CrosshairSpread;
                    Color crosshairColor = AmmoTracking.OldCursorColor; // Change color if necessary
                    Color crosshairBorderColor = AmmoTracking.OldCursorBorderColor;
                    Rectangle crosshairSourceRect = Crosshair.Bounds;
                    Rectangle crosshairBorderTargetRect = CrosshairBorder.Bounds;

                    Vector2[] directions =
                    [
                        new(0, -1),  // Top
                        new(0, 1),   // Bottom
                        new(-1, 0),  // Left
                        new(1, 0)    // Right
                    ];

                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 offset = directions[i] * offsetDistance;
                        Vector2 position = new Vector2(Main.mouseX, Main.mouseY) + offset;
                        float rotation = (i < 2) ? 0f : (float)Math.PI / 2;
                        spriteBatch.Draw(CrosshairBorder, position, crosshairBorderTargetRect, crosshairBorderColor, rotation, crosshairBorderTargetRect.Size() / 2, 1f, SpriteEffects.None, 0);
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 offset = directions[i] * offsetDistance;
                        Vector2 position = new Vector2(Main.mouseX, Main.mouseY) + offset;
                        float rotation = (i < 2) ? 0f : (float)Math.PI / 2;
                        spriteBatch.Draw(Crosshair, position, crosshairSourceRect, crosshairColor, rotation, crosshairSourceRect.Size() / 2, 1f, SpriteEffects.None, 0);
                    }

                    if (CrosshairSpread > 1 && Item.ModItem is not Shotgun)                    
                        CrosshairSpread -= 0.1f;
                    else if (CrosshairSpread > 1 && Item.ModItem is Shotgun && !Player.channel)                    
                        CrosshairSpread -= 0.1f;                        
                    
                }
            }
            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            AmmoArea.Left.Set(Main.mouseX - 35, 0f);
            AmmoArea.Top.Set(Main.mouseY, 0f);
            Recalculate();
            base.DrawSelf(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            bool overGrave = Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType == TileID.Tombstones;
            if (!HoldingInsurgencyWeapon || !AmmoTracking.isActive || overGrave || Player.mouseInterface || OverFriendlyNPC)
                return;

            string IconAmmo;
            if (ContentSamples.ItemsByType.TryGetValue(AmmoTracking.AmmoType, out Item Ammo))
            {
                if (Ammo != null && AmmoTracking.HasGL && AmmoTracking.AmmoTypeGL != -1 && ContentSamples.ItemsByType.TryGetValue(AmmoTracking.AmmoTypeGL, out Item AmmoGL))
                {
                    IconAmmo = $"[i:InsurgencyWeapons/{Ammo.ModItem?.Name}]";
                    string IconGrenade = $"[i:InsurgencyWeapons/{AmmoGL.ModItem?.Name}]";
                    AmmoDisplayText.SetText(AmmoTracking.CurrentAmmo + " / " + Player.CountItem(AmmoTracking.AmmoType) + IconAmmo + AmmoTracking.GrenadeName + Player.CountItem(AmmoTracking.AmmoTypeGL) + IconGrenade);
                }
                else if (Ammo != null)
                {
                    IconAmmo = $"[i:InsurgencyWeapons/{Ammo.ModItem?.Name}]";
                    AmmoDisplayText.SetText(AmmoTracking.CurrentAmmo + " / " + Player.CountItem(AmmoTracking.AmmoType) + IconAmmo);
                }
            }
            base.Update(gameTime);
        }
    }
}