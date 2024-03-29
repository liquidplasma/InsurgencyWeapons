﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.WeaponMagazines;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Rifles;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace InsurgencyWeapons
{
    public class InsurgencyGlobalProjectile : GlobalProjectile
    {
        private static int[] IgnoredDraws = {
            ModContent.ProjectileType<M1GarandEnbloc>(),
            ModContent.ProjectileType<EnfieldBlock>(),
        };

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.ModProjectile is not null and MagazineBase)
            {
                if (IgnoredDraws.Contains(projectile.type))
                    return base.PreDraw(projectile, ref lightColor);

                Texture2D texture = projectile.MyTexture();
                Rectangle rect = texture.Bounds;
                ExtensionMethods.BetterEntityDraw(texture, projectile.Center, rect, lightColor, projectile.rotation, rect.Size() / 2, projectile.scale, SpriteEffects.None);
                return false;
            }
            return base.PreDraw(projectile, ref lightColor);
        }
    }
}