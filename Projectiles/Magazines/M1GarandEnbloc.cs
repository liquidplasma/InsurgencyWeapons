using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;

namespace InsurgencyWeapons.Projectiles.Magazines
{
    internal class M1GarandEnbloc : MagazineBase
    {
        private static SoundStyle Tink => new("InsurgencyWeapons/Sounds/Weapons/Ins2/garand/gclip", 3)
        {
            Volume = 0.25f,
        };

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.penetrate = 5;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Projectile.MyTexture();
            Rectangle rect = texture.Bounds;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Color.White, Projectile.rotation, texture.Size() / 2, 0.75f, SpriteEffects.None);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(Tink, Projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }
    }
}