using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class MoonLord_EyeLaser: ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Last Eye Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 36;
            Projectile.timeLeft = 75;
            Projectile.penetrate = -1;
            Projectile.light = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Vector2 projDir = MathHelp.Normalize(Projectile.velocity);
            if (Projectile.timeLeft < 50)
            {
                Projectile.velocity *= 0.65f;
                projDir *=-1;
                Projectile.rotation =MathHelp.Lerp(Projectile.rotation, (float)Math.Atan2(projDir.Y, projDir.X),0.3f);
            }
            else
            {
                Projectile.rotation = (float)Math.Atan2(projDir.Y, projDir.X);
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }

    }
}
