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
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;

            Projectile.ai[0] = Projectile.timeLeft;
        }

        public override void AI()
        {
            Projectile.alpha = (int)((1-(Projectile.timeLeft/ Projectile.ai[0])) * 250);
        }

    }
}
