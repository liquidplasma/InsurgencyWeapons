using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace InsurgencyWeapons.Helpers
{
    public static class ShaderStuff
    {
        #region

        public static void FancyTracer(VertexStrip _vertexStrip, Projectile projectile)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(projectile.oldPos, projectile.oldRot, ShaderStuff.WhiteTrail, ShaderStuff.NormalBulletStripWidth, -Main.screenPosition + projectile.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        //Strip Width
        public static float NormalBulletStripWidth(float progressOnStrip)
        {
            float num = 0.4f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 4f, num);
        }

        #endregion

        #region

        //Color
        public static Color WhiteTrail(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.White, Color.GhostWhite, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = (byte)(result.A / 2);
            return result;
        }

        #endregion
    }
}