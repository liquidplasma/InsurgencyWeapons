using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;

namespace InsurgencyWeapons.Projectiles.WeaponMagazines.Casings
{
    internal class Shells : MagazineBase
    {
        private int Frame => (int)Projectile.ai[0];

        private SoundStyle Tink => new("InsurgencyWeapons/Sounds/Weapons/Shells/sshell", 6)
        {
            Volume = 0.4f,
        };

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.penetrate = 8;
            Projectile.timeLeft = 750;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D myTexture = Projectile.MyTexture();
            Rectangle rect = myTexture.Frame(verticalFrames: Main.projFrames[Type], frameY: Frame);
            ExtensionMethods.BetterEntityDraw(myTexture, Projectile.Bottom, rect, lightColor, Projectile.rotation, rect.Size() / 2, 1f, SpriteEffects.None, 0);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(Tink, Projectile.Center);
            return base.OnTileCollide(oldVelocity);
        }
    }
}