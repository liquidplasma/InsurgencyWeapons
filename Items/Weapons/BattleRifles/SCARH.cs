using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.BattleRifles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.BattleRifles
{
    internal class SCARH : BattleRifle
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
            Item.width = 82;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 13;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<SCARHHeld>();
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(350);
        }
    }
}