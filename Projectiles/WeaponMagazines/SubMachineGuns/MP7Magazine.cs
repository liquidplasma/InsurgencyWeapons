﻿namespace InsurgencyWeapons.Projectiles.WeaponMagazines.SubMachineGuns
{
    public class MP7Magazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }
    }
}