using InsurgencyWeapons.Projectiles.SubMachineGuns;

namespace InsurgencyWeapons.Items.Weapons.SubMachineGuns
{
    public class UMP45 : SubMachineGun
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 7;
            Item.width = 58;
            Item.height = 26;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 9;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<UMP45Held>();
            MoneyCost = 165;
            base.SetDefaults();
        }
    }
}