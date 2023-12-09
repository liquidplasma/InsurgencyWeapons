/*using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Revolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace InsurgencyWeapons.Items.Weapons.Revolvers
{
    /// <summary>
    /// Webley Mk.VI .455 WMk.II
    /// </summary>
    internal class Webley : Revolver
    {
        private int WebleyType => ModContent.ProjectileType<M29Held>();

        public override void SetStaticDefaults()
        {
            ItemID.Sets.gunProj[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 19;
            Item.width = 34;
            Item.height = 20;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 27;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[WebleyType] < 1)
            {
                int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Item.damage);
                Projectile gun = ExtensionMethods.BetterNewProjectile(player, player.GetSource_ItemUse_WithPotentialAmmo(Item, Item.useAmmo), player.Center, Vector2.Zero, WebleyType, Item.damage, Item.knockBack, player.whoAmI);
                gun.originalDamage = damage;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(160);
        }
    }
}*/