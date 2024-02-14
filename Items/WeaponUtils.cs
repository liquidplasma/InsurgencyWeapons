using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static InsurgencyWeapons.Helpers.ExtensionMethods;

namespace InsurgencyWeapons.Items
{
    internal abstract class AmmoItem : ModItem
    {
        public int MoneyCost { get; set; }
        public int CraftStack { get; set; }

        public override void SetDefaults()
        {
            if (MoneyCost == 0 || CraftStack == 0)
                throw new ArgumentException("MoneyCost or CraftStack property can't be 0");

            Item.value = MoneyCost;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 60;
        }

        public override bool CanStackInWorld(Item source)
        {
            return true;
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeAmmo(MoneyCost, CraftStack);
        }
    }

    /// <summary>
    /// Main class
    /// </summary>
    internal abstract class WeaponUtils : ModItem
    {
        public int WeaponHeldProjectile { get; set; }
        public int MoneyCost { get; set; }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.gunProj[Type] = true;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            if (MoneyCost == 0 && this is not Grenade)
                throw new ArgumentException("MoneyCost property can't be 0");

            Item.value = MoneyCost;
            base.SetDefaults();
        }

        public override void HoldItem(Player player)
        {
            if (WeaponHeldProjectile != 0 && player.ownedProjectileCounts[WeaponHeldProjectile] < 1)
            {
                int damage = (int)player.GetTotalDamage(DamageClass.Ranged).ApplyTo(Item.damage);
                Projectile gun = BetterNewProjectile(player, player.GetSource_ItemUse_WithPotentialAmmo(Item, Item.useAmmo), player.Center, Vector2.Zero, WeaponHeldProjectile, Item.damage, Item.knockBack, player.whoAmI);
                gun.originalDamage = damage;
            }
            base.HoldItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override void AddRecipes()
        {
            this.RegisterINS2RecipeWeapon(MoneyCost);
            base.AddRecipes();
        }
    }

    internal abstract class AssaultRifle : WeaponUtils
    { }

    internal abstract class BattleRifle : WeaponUtils
    { }

    internal abstract class Carbine : WeaponUtils
    { }

    internal abstract class Rifle : WeaponUtils
    { }

    internal abstract class Shotgun : WeaponUtils
    { }

    internal abstract class SniperRifle : WeaponUtils
    { }

    internal abstract class SubMachineGun : WeaponUtils
    { }

    internal abstract class LightMachineGun : WeaponUtils
    { }

    internal abstract class Launcher : WeaponUtils
    { }

    internal abstract class Pistol : WeaponUtils
    { }

    internal abstract class Revolver : WeaponUtils
    { }

    internal abstract class Grenade : WeaponUtils
    {
        public override void SetDefaults()
        {
            WeaponHeldProjectile = 0;
            base.SetDefaults();
        }
    }
}