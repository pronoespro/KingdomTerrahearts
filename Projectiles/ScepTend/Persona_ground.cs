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
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 18;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.rotation = (float)Math.PI/2f;
            Projectile.ai[1] = 1;
            Projectile.ai[0] = 3;
        }

        public override void AI()
        {
            Projectile.alpha = (int)((1f - Projectile.timeLeft / 100f) * 200f)+50;
            Projectile.frame = (int)((Projectile.ai[0] / 20f) * 4);
            Projectile.ai[0]+=Projectile.ai[1];
            if(Projectile.ai[0]>=17.5f || Projectile.ai[0] <= 2.5f)
            {
                Projectile.ai[1] *= -1;
            }
        }
    }
}
