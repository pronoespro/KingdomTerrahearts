using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class roxasLightBeam : ModProjectile
    {

        float timeLeft = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Beam");
        }

        public override void SetDefaults()
        {

            projectile.width = 30;
            projectile.height = 120;
            projectile.aiStyle = -1;
            projectile.alpha = 100;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.light = 1.5f;
            projectile.maxPenetrate = -1;
            projectile.melee = true;
            projectile.penetrate = 1000;
            projectile.tileCollide = false;
            projectile.timeLeft = 75;

        }

        public override void AI()
        {
            projectile.velocity.X = projectile.velocity.X + projectile.velocity.Y*MathHelp.Sign(projectile.velocity.X);
            projectile.velocity.Y = 0;

            timeLeft = (timeLeft == 0) ? projectile.timeLeft : timeLeft;

            projectile.alpha = (projectile.timeLeft > 50) ? 100 : 100 + 155 * (projectile.timeLeft);
        }

    }
}
