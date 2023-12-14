using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Revolvers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.Revolvers
{
    internal class M29 : Revolver
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
            WeaponHeldProjectile = ModContent.ProjectileType<M29Held>();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(190);
        }
    }
}