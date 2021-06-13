using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Vergil_Bubble:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 30;
            projectile.velocity = Vector2.Zero;
            projectile.damage = 0;
            projectile.ai[0] = (projectile.ai[0] == 0) ? 1 : projectile.ai[0];
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC || !target.friendly;
        }

        public override void AI()
        {
            projectile.alpha = (int)(250 - (projectile.timeLeft / 30f)*5);
            projectile.damage = 0;
            projectile.scale =(3-(1-(projectile.timeLeft/30f)/5))*projectile.ai[0];
            projectile.frame = (int)((1f - (projectile.timeLeft / 30f)) * 4f);

            for (int i = 0; i < 15; i++)
            {
                if (Main.rand.Next(3) == 0)
                {
                    Vector2 scale = new Vector2(projectile.width * projectile.scale, projectile.height * projectile.scale);
                    Dust.NewDust(projectile.Center - scale / 4, (int)scale.X, (int)scale.Y, DustID.Electric);
                }
            }

        }

    }
}
