﻿using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Shotguns;

namespace InsurgencyWeapons.Items.Weapons.Shotguns
{
    /// <summary>
    /// Ithaca M37 12 Gauge
    /// </summary>
    public class Ithaca : Shotgun
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<TwelveGauge>(), Type);
            AmmoItem.AddRelationShip(ModContent.ItemType<TwelveGaugeSlug>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 6f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 45;
            Item.width = 82;
            Item.height = 18;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 14;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<IthacaHeld>();
            MoneyCost = 275;
            base.SetDefaults();
        }
    }
}