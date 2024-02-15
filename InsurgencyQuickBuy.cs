using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using InsurgencyWeapons.Projectiles;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    internal class InsurgencyQuickBuy : ModPlayer
    {
        private bool HoldingInsurgencyWeapon => Player.HoldingInsurgencyWeapon();
        private Item HeldItem => Player.HeldItem;

        private Item Money
        {
            get
            {
                if (Player.HasItem(Insurgency.Money))
                {
                    return Player.FindItemInInventory(Insurgency.Money);
                }
                return null;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Money != null && HoldingInsurgencyWeapon && InsurgencyModKeyBind.QuickBuy.JustPressed)
            {
                WeaponUtils GrabProjType = (WeaponUtils)HeldItem.ModItem;
                int projIndex = HelperStats.FindProjectileIndex(Player, GrabProjType.WeaponHeldProjectile);
                if (Main.projectile.IndexInRange(projIndex))
                {
                    Projectile HeldWeapon = Main.projectile[projIndex];
                    WeaponBase GrabAmmoType = (WeaponBase)HeldWeapon.ModProjectile;
                    AmmoItem AmmoCost = (AmmoItem)ContentSamples.ItemsByType[GrabAmmoType.AmmoType].ModItem;
                    int resultingCost = (int)(AmmoCost.CraftStack * 1.2f);
                    if (Money.stack >= resultingCost)
                    {
                        Money.stack -= resultingCost;
                        SoundEngine.PlaySound(InsurgencyGlobalItem.AmmoNoise, Player.Center);
                        Player.QuickSpawnItem(Player.GetSource_DropAsItem(), AmmoCost.Type, AmmoCost.CraftStack);
                    }
                    if (Money != null && Money.stack <= 0)
                        Money.TurnToAir();
                }
            }
            base.ProcessTriggers(triggersSet);
        }
    }
}