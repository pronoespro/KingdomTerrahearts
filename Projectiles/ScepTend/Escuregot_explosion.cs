using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Escuregot_explosion:ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 30;

            projectile.ai[0] = projectile.timeLeft;
        }

        public override void AI()
        {
            projectile.alpha = (int)((1-(projectile.timeLeft/ projectile.ai[0])) * 250);
        }

    }
}
