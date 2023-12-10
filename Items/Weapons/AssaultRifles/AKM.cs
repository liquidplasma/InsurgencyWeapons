using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.AssaultRifles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.AssaultRifles
{
    /// <summary>
    /// AKM 7.62x39mm + GP-25 40mm VOG-25P
    /// </summary>
    internal class AKM : AssaultRifle
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
            Item.useAnimation = Item.useTime = 6;
            Item.width = 72;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 12;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<AKMHeld>();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(350);
        }
    }
}