﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.SubMachineGuns;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.SubMachineGuns
{
    internal class PPSH41 : SubMachineGun
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.gunProj[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 4;
            Item.width = 42;
            Item.height = 28;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 7;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<PPSH41Held>();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(235);
        }
    }
}