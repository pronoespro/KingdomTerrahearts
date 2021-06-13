using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Summons
{
    public class Simba:SummonBase
    {

        float velX;
        bool grounded;
        int[] attackTimers = new int[] {100,100,250 };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bambi");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 25;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.scale = 2f;
            projectile.light = 1f;
            projectile.timeLeft = 2000;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            grounded = true;
            return false;
        }

        public override void AI()
        {

            TargetEnemie();

            if (target != -1)
            {

                if (Vector2.Distance(projectile.Center, Main.npc[target].Center) < 400 || projectile.ai[0]==2)
                {
                    if (Math.Abs(projectile.velocity.X) < 1)
                    {
                        projectile.ai[1]++;


                        if (projectile.ai[1] > attackTimers[(int)projectile.ai[0]])
                        {
                            if (projectile.ai[0] == 0)
                            {
                                projectile.ai[0] = Main.rand.Next(1, 3);
                            }
                            else
                            {
                                if(projectile.ai[0]==1 && Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                                {
                                    Filters.Scene["Shockwave"].Deactivate();
                                }
                                projectile.ai[0] = 0;
                            }
                            projectile.ai[1] = 0;
                        }

                        switch (projectile.ai[0])
                        {
                            case 0:
                                projectile.velocity.Y += 0.5f;
                                projectile.rotation = (grounded) ? 0 : (projectile.velocity.Y > 0) ? (float)Math.PI / 2:(float)Math.PI*1.5f;
                                projectile.frame = 0;
                                projectile.rotation = 0;
                                break;
                            case 1:
                                
                                if (projectile.frame != 1)
                                {
                                    projectile.frame = 1;
                                    //damage enemies
                                    for (int i = 0; i < Main.maxNPCs; i++)
                                    {
                                        if (KingdomTerrahearts.instance.IsEnemy(i))
                                        {
                                            Main.npc[i].life -= projectile.damage/2;
                                            Main.npc[i].checkDead();
                                        }
                                    }
                                    if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive())
                                    {
                                        Filters.Scene.Activate("Shockwave", projectile.Center).GetShader().UseColor(3, 15, 25).UseTargetPosition(projectile.Center);
                                    }
                                }
                                if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                                {
                                    float progress = (100f - projectile.ai[1]) / attackTimers[1]/2; // Will range from -3 to 3, 0 being the point where the bomb explodes.
                                    Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(150 * (1 - progress / 3f));
                                }
                                //stun enemies
                                for (int i = 0; i < Main.maxNPCs; i++)
                                {
                                    if (KingdomTerrahearts.instance.IsEnemy(i))
                                    {
                                        Main.npc[i].velocity = Vector2.Zero;

                                        Main.npc[i].life -= projectile.damage / 5;
                                        Main.npc[i].checkDead();
                                    }
                                }

                                projectile.velocity.X = 0;
                                break;
                            case 2:

                                if (projectile.frame != 3)
                                {
                                    projectile.frame = 3;
                                    projectile.velocity.Y = -15;
                                }

                                projectile.rotation += (float)Math.PI * 0.35f;

                                for(int i = 0; i < Main.rand.Next(0, 5);i++)
                                {
                                    Dust.NewDust(projectile.position, (int)(projectile.width), (int)(projectile.height), DustID.Fire);
                                }

                                for (int i = 0; i < Main.maxNPCs; i++)
                                {
                                    if (KingdomTerrahearts.instance.IsEnemy(i))
                                    {
                                        Main.npc[i].AddBuff(BuffID.OnFire, 15);
                                    }
                                }

                                if (projectile.velocity.Y > 0)
                                {
                                    for (int i = 0; i < Main.maxNPCs; i++)
                                    {
                                        if (KingdomTerrahearts.instance.IsEnemy(i))
                                        {
                                            Main.npc[i].life -= projectile.damage / 10;
                                            Main.npc[i].checkDead();
                                        }
                                    }
                                }
                                else
                                {
                                    projectile.velocity.Y +=  0.25f ;
                                }
                                break;
                        }
                    }
                    else
                    {
                        projectile.velocity.X -= projectile.velocity.X / Math.Abs(projectile.velocity.X) * 0.5f;
                        projectile.frame = 3;
                        projectile.rotation += (float)(0.1f * Math.PI);
                    }
                }
                else
                {
                    if (projectile.ai[0] != 0 || projectile.ai[1]!=0)
                    {
                        if (projectile.ai[0] == 1 && Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                        {
                            Filters.Scene["Shockwave"].Deactivate();
                        }
                        projectile.ai[0] = projectile.ai[1] = 0;
                    }
                    projectile.frame = 2;
                    if (Math.Abs((Main.npc[target].Center - projectile.Center).X) > Math.Abs((Main.npc[target].Center - projectile.Center).Y))
                    {
                        projectile.rotation = 0;
                        velX = (Main.npc[target].Center - projectile.Center).X;
                        projectile.velocity = new Vector2(velX / Math.Abs(velX) * 15, 0);
                    }
                    else
                    {
                        projectile.rotation = (float)Math.PI/2;
                        velX = (Main.npc[target].Center - projectile.Center).Y;
                        projectile.velocity = new Vector2(0,velX / Math.Abs(velX) * 15);
                    }
                }

            }
            else
            {
                if (projectile.ai[0] != 0 || projectile.ai[1] != 0)
                {
                    if (projectile.ai[0] == 1 && Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                    {
                        Filters.Scene["Shockwave"].Deactivate();
                    }
                    projectile.ai[0] = projectile.ai[1] = 0;
                }
                if (Vector2.Distance(projectile.Center, Main.player[projectile.owner].Center) < 300)
                {
                    if (Math.Abs(projectile.velocity.X) < 1)
                    {
                        projectile.frame = 0;
                        projectile.velocity.X = 0;
                        projectile.rotation = 0;
                        projectile.velocity.Y += 0.25f;
                    }
                    else
                    {
                        projectile.velocity.X -= projectile.velocity.X / Math.Abs(projectile.velocity.X)*0.5f;
                        projectile.frame = 3;
                        projectile.rotation += (float)(0.1f * Math.PI); 
                    }
                }
                else
                {
                    projectile.frame = 2;
                    if (Math.Abs((Main.player[projectile.owner].Center - projectile.Center).X) > Math.Abs((Main.player[projectile.owner].Center - projectile.Center).Y))
                    {
                        velX = (Main.player[projectile.owner].Center - projectile.Center).X;
                        projectile.velocity = new Vector2(velX / Math.Abs(velX) * 15, 0);
                    }
                    else
                    {
                        projectile.rotation = (float)Math.PI / 2;
                        velX = (Main.player[projectile.owner].Center - projectile.Center).Y;
                        projectile.velocity = new Vector2(0, velX / Math.Abs(velX) * 15);
                    }
                }
            }


            grounded = false;
        }

    }
}
