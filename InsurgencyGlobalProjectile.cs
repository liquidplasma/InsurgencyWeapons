﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Projectiles.WeaponMagazines;
using InsurgencyWeapons.Projectiles.WeaponMagazines.Rifles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    internal class InsurgencyGlobalProjectile : GlobalProjectile
    {
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.ModProjectile is not null and MagazineBase)
            {
                if (projectile.type == ModContent.ProjectileType<M1GarandEnbloc>())
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