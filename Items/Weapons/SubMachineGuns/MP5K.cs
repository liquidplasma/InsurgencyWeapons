﻿using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.SubMachineGuns;

namespace InsurgencyWeapons.Items.Weapons.SubMachineGuns
{
    public class MP5K : SubMachineGun
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<Bullet919>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 4;
            Item.width = 40;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 9;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<MP5KHeld>();
            MoneyCost = 175;
            base.SetDefaults();
        }
    }
}