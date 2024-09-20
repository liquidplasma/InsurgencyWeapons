using InsurgencyWeapons.Projectiles.SubMachineGuns;

namespace InsurgencyWeapons.Items.Weapons.SubMachineGuns
{
    /// <summary>
    /// Thompson M1928 .45ACP
    /// </summary>
    public class ChicagoTypewriter : SubMachineGun
    {
        public override void SetDefaults()
        {
            Item.knockBack = 4f;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = Item.useTime = 6;
            Item.width = 74;
            Item.height = 24;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.damage = 10;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            WeaponHeldProjectile = ModContent.ProjectileType<ChicagoTypewriterHeld>();
            MoneyCost = 225;
            base.SetDefaults();
        }
    }
}