using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.SniperRifles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.SniperRifles
{
    internal class Mosin : SniperRifle
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.gunProj[Type] = true;
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
            Item.damage = 64;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<MosinHeld>();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(320);
        }
    }
}