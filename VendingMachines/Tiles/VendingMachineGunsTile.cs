using Microsoft.Xna.Framework.Graphics;

namespace InsurgencyWeapons.VendingMachines.Tiles
{
    public class VendingMachineGunsTile : VendingMachineTile
    {
        private float strength;

        private bool increase;

        public override void SetStaticDefaults()
        {
            AddMapEntry(Color.Red);
            base.SetStaticDefaults();
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (increase)
            {
                strength += 0.001f;
                if (strength >= 0.75f)
                    increase = false;
            }
            else
            {
                strength -= 0.001f;
                if (strength <= 0.1f)
                    increase = true;
            }
            Vector2 pos = new Vector2(i, j) * 16;
            Lighting.AddLight(pos, Color.MediumVioletRed.ToVector3() * strength);
            return base.PreDraw(i, j, spriteBatch);
        }
    }
}