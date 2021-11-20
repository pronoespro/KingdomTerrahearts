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
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1000;
            Projectile.timeLeft = 150;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void AI()
        {
            Projectile.scale = 3f;
            if (MathHelp.Magnitude(Projectile.velocity) > 0)
            {
                Projectile.velocity.Y += 0.1f;
                Projectile.frame = (Projectile.velocity.Y==0)?0:((Projectile.velocity.Y>0)?4:5);
            }
            else
            {
                Projectile.frame = 0;
            }

            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].townNPC)
                {
                    if (Vector2.Distance(Main.npc[i].Center, Projectile.Center) < (Main.npc[i].width + Main.npc[i].height) / 2 + (Projectile.width+Projectile.height)/2)
                    {
                        Main.npc[i].life = 0;
                        Main.npc[i].checkDead();
                    }
                }
            }
        }


    }
}
