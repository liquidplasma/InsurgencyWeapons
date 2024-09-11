﻿using InsurgencyWeapons.Projectiles.SniperRifles;

namespace InsurgencyWeapons.Items.Weapons.SniperRifles
{
    /// <summary>
    /// Gewehr 43 7.92x57mm + ZF4 Scope
    /// </summary>
    public class G43 : SniperRifle
    {
        public override void SetDefaults()
        {
            Item.crit = 17;
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 24;
            Item.width = 108;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 55;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 7, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<G43Held>();
            MoneyCost = 340;
            base.SetDefaults();
        }
    }
}