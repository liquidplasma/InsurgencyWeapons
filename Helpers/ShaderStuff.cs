using Microsoft.Xna.Framework;
using Terraria;

namespace InsurgencyWeapons.Helpers
{
    internal static class ShaderStuff
    {
        #region

        //Strip Widths
        public static float NormalBulletStripWidth(float progressOnStrip)
        {
            float num = 0.9f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 4f, num);
        }

        public static float GhostlyArrowStripWidth(float progressOnStrip)
        {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 12f, num);
        }

        public static float UfoShotStripWidth(float progressOnStrip)
        {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 12f, num);
        }

        public static float RainbowRodStripWidth(float progressOnStrip)
        {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 32f, num);
        }

        public static float PulseBulletStripWidth(float progressOnStrip)
        {
            return MathHelper.Lerp(13f, 23f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
        }

        public static float MagicMissileStripWidth(float progressOnStrip)
        {
            return MathHelper.Lerp(26f, 32f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
        }

        #endregion

        #region

        //Colors
        public static Color WhiteTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.White, Color.AntiqueWhite, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }

        public static Color GoldenTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Gold, Color.DarkGoldenrod, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }

        public static Color GreenTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Green, Color.LimeGreen, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }

        public static Color YellowTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Yellow, Color.GreenYellow, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }

        public static Color BlueTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Blue, Color.CadetBlue, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }

        public static Color UfoShotRedTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Red, Color.DarkRed, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }

        public static Color PulseBulletTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Yellow, Color.Cyan, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }

        public static Color PulseBallTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Cyan, Color.DarkCyan, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }

        #endregion
    }
}