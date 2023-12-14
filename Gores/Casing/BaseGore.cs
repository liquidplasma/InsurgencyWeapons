using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace InsurgencyWeapons.Gores.Casing
{
    internal abstract class BaseGore : ModGore
    {
        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
        }

        public override void OnSpawn(Terraria.Gore gore, IEntitySource source)
        {
            gore.timeLeft = InsurgencyModConfig.Instance.CasingLifeTime * 60;
            gore.behindTiles = true;
            gore.alpha = 255;
            //UpdateType = 277;
            base.OnSpawn(gore, source);
        }

        public override bool Update(Gore gore)
        {
            gore.alpha -= 16;
            gore.velocity.X *= 0.96f;
            if (gore.timeLeft == 0)
                gore.active = false;
            return base.Update(gore);
        }
    }
}