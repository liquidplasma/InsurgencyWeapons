﻿using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.AssaultRifles;

namespace InsurgencyWeapons.Items.Weapons.AssaultRifles
{
    public class Groza : AssaultRifle
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet939>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 6;
            Item.width = 68;
            Item.height = 30;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 9;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<GrozaHeld>();
            MoneyCost = 320;
            base.SetDefaults();
        }
    }
}