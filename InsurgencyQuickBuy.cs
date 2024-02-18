﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using InsurgencyWeapons.Projectiles;
using Terraria.GameInput;

namespace InsurgencyWeapons
{
    internal class InsurgencyQuickBuy : ModPlayer
    {
        private bool HoldingInsurgencyWeapon => Player.HoldingInsurgencyWeapon();
        private Item HeldItem => Player.HeldItem;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.HasItem(Insurgency.Money) && HoldingInsurgencyWeapon && InsurgencyModKeyBind.QuickBuy.JustPressed)
            {
                WeaponUtils GrabProjType = (WeaponUtils)HeldItem.ModItem;
                int projIndex = HelperStats.FindProjectileIndex(Player, GrabProjType.WeaponHeldProjectile);
                if (Main.projectile.IndexInRange(projIndex))
                {
                    Projectile HeldWeapon = Main.projectile[projIndex];
                    WeaponBase GrabAmmoType = (WeaponBase)HeldWeapon.ModProjectile;
                    AmmoItem AmmoCost = (AmmoItem)ContentSamples.ItemsByType[GrabAmmoType.AmmoType].ModItem;
                    int resultingCost = (int)(AmmoCost.CraftStack * 1.2f);
                    if (Player.CountItem(Insurgency.Money) <= resultingCost)
                        return;

                    for (int i = 0; i < resultingCost; i++)
                    {
                        Player.ConsumeItem(Insurgency.Money);
                    }
                    SoundEngine.PlaySound(InsurgencyGlobalItem.AmmoNoise, Player.Center);
                    Player.QuickSpawnItem(Player.GetSource_DropAsItem(), AmmoCost.Type, AmmoCost.CraftStack);
                }
            }
            base.ProcessTriggers(triggersSet);
        }
    }
}