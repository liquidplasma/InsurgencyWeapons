﻿using InsurgencyWeapons.Items.Ammo;
using InsurgencyWeapons.Projectiles.Launchers;

namespace InsurgencyWeapons.Items.Weapons.Launchers
{
    public class Panzerfaust : Launcher
    {
        public override void SetStaticDefaults()
        {
            AmmoItem.AddRelationShip(ModContent.ItemType<PZFaustRocket>(), Type);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 40;
            Item.width = 108;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 210;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<PanzerfaustHeld>();
            MoneyCost = 400;
            base.SetDefaults();
        }
    }
}