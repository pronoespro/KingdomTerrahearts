using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class zafiProtector:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1000;
            projectile.timeLeft = 150;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void AI()
        {
            projectile.scale = 3f;
            if (MathHelp.Magnitude(projectile.velocity) > 0)
            {
                projectile.velocity.Y += 0.1f;
                projectile.frame = (projectile.velocity.Y==0)?0:((projectile.velocity.Y>0)?4:5);
            }
            else
            {
                projectile.frame = 0;
            }

            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].townNPC)
                {
                    if (Vector2.Distance(Main.npc[i].Center, projectile.Center) < (Main.npc[i].width + Main.npc[i].height) / 2 + (projectile.width+projectile.height)/2)
                    {
                        Main.npc[i].life = 0;
                        Main.npc[i].checkDead();
                    }
                }
            }
        }


    }
}
