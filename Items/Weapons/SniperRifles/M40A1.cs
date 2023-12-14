using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.SniperRifles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.SniperRifles
{
    internal class M40A1 : SniperRifle
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
            Item.useAnimation = Item.useTime = 40;
            Item.width = 80;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 57;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<M40A1Held>();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(305);
        }
    }
}