using Microsoft.Xna.Framework;
using Terraria;

namespace InsurgencyWeapons.Helpers
{
    internal static class ShaderStuff
    {
        #region
        //Strip Width
        public static float NormalBulletStripWidth(float progressOnStrip)
        {
            float num = 0.9f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 4f, num);
        }
        #endregion

        #region
        //Color
        public static Color WhiteTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.White, Color.AntiqueWhite, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }        
        #endregion
    }
}