using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Rifles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.Rifles
{
    /// <summary>
    /// SVT-40 7.62x54mmR
    /// </summary>
    internal class SVT40 : Rifle
    {
        private int SVTType => ModContent.ProjectileType<SVT40Held>();

        public override void SetStaticDefaults()
        {
            ItemID.Sets.gunProj[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 15;
            Item.width = 80;
            Item.height = 16;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 18;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[SVTType] < 1)
            {
                int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Item.damage);
                Projectile gun = ExtensionMethods.BetterNewProjectile(player, player.GetSource_ItemUse_WithPotentialAmmo(Item, Item.useAmmo), player.Center, Vector2.Zero, SVTType, Item.damage, Item.knockBack, player.whoAmI);
                gun.originalDamage = damage;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(290);
        }
    }
}