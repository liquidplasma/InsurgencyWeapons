﻿using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Shotguns;

namespace InsurgencyWeapons.Items.Weapons.Shotguns
{
    /// <summary>
    /// Benelli M1014 12 Gauge
    /// </summary>
    public class M1014 : Shotgun
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
            Item.useAnimation = Item.useTime = 9;
            Item.width = 78;
            Item.height = 20;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 13;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M1014Held>();
            MoneyCost = 330;
            base.SetDefaults();
        }
    }
}