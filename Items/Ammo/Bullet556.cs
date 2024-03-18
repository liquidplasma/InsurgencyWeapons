using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencyWeapons.Items.Ammo
{
    public class Bullet556 : AmmoItem
    {
        public override void SetDefaults()
        {
            MoneyCost = 30;
            CraftStack = 30;
            Item.width = 7;
            Item.height = 27;
            Item.DefaultsToInsurgencyAmmo(5);
            base.SetDefaults();
        }
    }
}