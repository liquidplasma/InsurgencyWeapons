using Terraria.GameContent;

namespace InsurgencyWeapons.Gores.Casing
{
    public abstract class BaseGore : ModGore
    {
        public override void SetStaticDefaults()
        {
            ChildSafety.SafeGore[Type] = true;
        }

        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            gore.timeLeft = InsurgencyModConfig.Instance.CasingLifeTime * 60;
            gore.behindTiles = true;
            gore.alpha = 250;
            //UpdateType = 277;
            base.OnSpawn(gore, source);
        }

        public override bool Update(Gore gore)
        {
            int dustTime = (int)(InsurgencyModConfig.Instance.CasingLifeTime * 60 * 0.9f);
            if (gore.alpha > 0)
                gore.alpha -= 10;
            gore.velocity.X *= 0.96f;
            gore.rotation += gore.velocity.X * 0.1f;
            if (gore.timeLeft == 0)
                gore.active = false;
            if (Main.rand.NextBool(10) && gore.timeLeft > dustTime)
            {
                Dust trail = Dust.NewDustDirect(gore.position, 2, 2, DustID.Smoke);
                trail.velocity *= 0f;
                trail.scale = 0.5f;
            }
            return base.Update(gore);
        }
    }
}