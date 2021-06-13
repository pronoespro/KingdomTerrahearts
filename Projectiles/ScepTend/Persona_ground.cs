using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Persona_ground : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 18;
            projectile.height = 50;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 100;
            projectile.rotation = (float)Math.PI/2f;
            projectile.ai[1] = 1;
            projectile.ai[0] = 3;
        }

        public override void AI()
        {
            projectile.alpha = (int)((1f - projectile.timeLeft / 100f) * 200f)+50;
            projectile.frame = (int)((projectile.ai[0] / 20f) * 4);
            projectile.ai[0]+=projectile.ai[1];
            if(projectile.ai[0]>=17.5f || projectile.ai[0] <= 2.5f)
            {
                projectile.ai[1] *= -1;
            }
        }
    }
}
