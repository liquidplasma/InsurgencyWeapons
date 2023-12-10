using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.SubMachineGuns;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.SubMachineGuns
{
    internal class MP7 : SubMachineGun
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
            Item.damage = 8;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<MP7Held>();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(315);
        }
    }
}