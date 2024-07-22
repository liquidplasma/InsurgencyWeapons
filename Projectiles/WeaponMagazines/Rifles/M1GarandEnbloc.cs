using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Rifles
{
    public class M1GarandEnbloc : MagazineBase
    {
        private static SoundStyle Tink => new("InsurgencyWeapons/Sounds/Weapons/Ins2/garand/gclip", 3)
        {
            Volume = 0.25f,
        };

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 5;
            Projectile.penetrate = 5;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Projectile.MyTexture();
            Rectangle rect = texture.Bounds;
            BetterEntityDraw(texture, Projectile.Center, rect, lightColor, Projectile.rotation, texture.Size() / 2, 0.5f, SpriteEffects.None);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(Tink, Projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }
    }
}