using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Summons
{
    public class Dumbo: SummonBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dumbo");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 78/2;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.scale = 2f;
            projectile.light = 1f;
            projectile.timeLeft = 1000;
        }

        public override void AI()
        {

            TargetEnemie();

            projectile.ai[0]++;
            projectile.frame = (int)(projectile.ai[0] / 50f)%2;

            if (target!=-1 && Vector2.Distance(Main.npc[target].Center, projectile.Center)<750)
            {
                if (Vector2.Distance(Main.npc[target].Center, projectile.Center) > 400)
                {
                    projectile.velocity = MathHelp.Normalize(Main.npc[target].Center - projectile.Center);

                    projectile.spriteDirection = (projectile.velocity.X == 0) ? projectile.spriteDirection : ((projectile.velocity.X > 0) ? 1 : -1);
                    projectile.rotation = (projectile.spriteDirection > 0) ? (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) : (float)(Math.PI + Math.Atan2(projectile.velocity.Y, projectile.velocity.X));
                }
                else
                {
                    projectile.spriteDirection = (Main.npc[target].Center - projectile.Center).X > 0 ? 1 : -1;
                    if (projectile.ai[0] % 2 == 0)
                    {
                        int proj = Projectile.NewProjectile(projectile.Center, MathHelp.Normalize(Main.npc[target].Center+new Vector2(0,25) - projectile.Center) * 5, ProjectileID.WaterStream, projectile.damage/4, 3, projectile.owner);
                        Main.projectile[proj].timeLeft = 50;

                        if (projectile.ai[0] % 120 == 0)
                        {
                            int itm=Item.NewItem(projectile.getRect(), ItemID.Star);
                            Main.item[itm].velocity = new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
                        }

                    }

                    projectile.velocity = Vector2.Zero;

                    projectile.rotation = 0;
                }
            }
            else
            {
                if(Vector2.Distance(Main.player[projectile.owner].Center, projectile.Center) > 200)
                {
                    projectile.velocity = MathHelp.Normalize(Main.player[projectile.owner].Center - projectile.Center);
                    if (Vector2.Distance(Main.player[projectile.owner].Center, projectile.Center) > 700)
                        projectile.timeLeft = 2;

                    projectile.spriteDirection = (projectile.velocity.X == 0) ? projectile.spriteDirection : ((projectile.velocity.X > 0) ? 1 : -1);
                    projectile.rotation = (projectile.spriteDirection > 0) ? (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) : (float)(Math.PI + Math.Atan2(projectile.velocity.Y, projectile.velocity.X));
                }
                else
                {
                    projectile.velocity = Vector2.Zero;
                    projectile.rotation = 0;
                }
            }


        }

    }
}
