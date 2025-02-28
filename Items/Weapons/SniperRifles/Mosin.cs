﻿using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.SniperRifles;

namespace InsurgencyWeapons.Items.Weapons.SniperRifles
{
    public class Mosin : SniperRifle
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet76254R>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.crit = 17;
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 48;
            Item.width = 78;
            Item.height = 22;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 96;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<MosinHeld>();
            MoneyCost = 320;
            base.SetDefaults();
        }
    }
}