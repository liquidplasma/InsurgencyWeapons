using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.Grenades;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Items.Weapons.Grenades
{
    internal class M24St : Grenade
    {
        private int M24StType => ModContent.ProjectileType<M24StExplosive>();
        private bool Fired;
        private int Timer;

        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 120;
            Item.width = 10;
            Item.height = 32;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 162;
            Item.shootSpeed = 6f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 0, 1, 5);
            Item.rare = ItemRarityID.LightRed;
            Item.maxStack = Item.CommonMaxStack;
            Item.DamageType = DamageClass.Ranged;
            base.SetDefaults();
        }

        public override void HoldItem(Player player)
        {
            if (Item.stack <= 0)
                Item.TurnToAir();

            Item.useTime = Item.useAnimation = 120;

            if (player.channel && !Fired)
                Fired = true;

            if (Fired)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    int mouseDirection = player.DirectionTo(Main.MouseWorld).X > 0f ? 1 : -1;
                    player.ChangeDir(mouseDirection);
                }
                Timer++;
                switch (Timer)
                {
                    case 10:
                        SoundEngine.PlaySound(Sounds.M24StCap, player.Center);
                        break;

                    case 50:
                        SoundEngine.PlaySound(Sounds.M24StRope, player.Center);
                        Item.useStyle = ItemUseStyleID.Swing;
                        break;

                    case 90:
                        SoundEngine.PlaySound(Sounds.M24StThrow, player.Center);
                        Item.stack--;
                        Vector2 aim = player.Center.DirectionTo(Main.MouseWorld) * Item.shootSpeed;
                        int damage = (int)player.GetTotalDamage(Item.DamageType).ApplyTo(Item.damage);
                        float knockback = (int)player.GetTotalKnockback(Item.DamageType).ApplyTo(Item.knockBack);
                        ExtensionMethods.BetterNewProjectile(player, player.GetSource_ItemUse(Item), player.Center, aim, M24StType, damage, knockback, player.whoAmI);

                        break;
                }
            }
            if (!player.channel && Fired && Timer > Item.useTime)
            {
                Timer = 0;
                Fired = false;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            base.HoldItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(45);
        }
    }
}