using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Rifles
{
    internal class EnfieldBlock : MagazineBase
    {        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 12;
            Projectile.penetrate = 5;
        }
    }
}
