﻿using InsurgencyWeapons.Projectiles.Pistols;

namespace InsurgencyWeapons.Items.Weapons.Pistols
{    
    public class USP : Pistol
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 7;
            Item.width = 30;
            Item.height = 20;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 17;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<USPHeld>();
            MoneyCost = 145;
            base.SetDefaults();
        }
    }
}