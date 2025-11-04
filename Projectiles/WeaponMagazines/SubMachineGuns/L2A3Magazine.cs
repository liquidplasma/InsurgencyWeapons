using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencyWeapons.Projectiles.WeaponMagazines.SubMachineGuns
{
    internal class L2A3Magazine : MagazineBase
    {
        public override void SetDefaults()
        {
            Projectile.scale = 0.6f;
            base.SetDefaults();
        }
    }
}