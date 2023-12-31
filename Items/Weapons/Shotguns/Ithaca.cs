﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Shotguns;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.Shotguns
{
    /// <summary>
    /// Ithaca M37 12 Gauge
    /// </summary>
    internal class Ithaca : Shotgun
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.gunProj[Type] = true;
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
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<IthacaHeld>();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(275);
        }
    }
}