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
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 25;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 2f;
            Projectile.light = 1f;
            Projectile.timeLeft = 2000;
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

                if (Vector2.Distance(Projectile.Center, Main.npc[target].Center) < 400 || Projectile.ai[0]==2)
                {
                    if (Math.Abs(Projectile.velocity.X) < 1)
                    {
                        Projectile.ai[1]++;


                        if (Projectile.ai[1] > attackTimers[(int)Projectile.ai[0]])
                        {
                            if (Projectile.ai[0] == 0)
                            {
                                Projectile.ai[0] = Main.rand.Next(1, 3);
                            }
                            else
                            {
                                /*
                                if(Projectile.ai[0]==1 && Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                                {
                                    Filters.Scene["Shockwave"].Deactivate();
                                }*/
                                Projectile.ai[0] = 0;
                            }
                            Projectile.ai[1] = 0;
                        }

                        switch (Projectile.ai[0])
                        {
                            case 0:
                                Projectile.velocity.Y += 0.5f;
                                Projectile.rotation = (grounded) ? 0 : (Projectile.velocity.Y > 0) ? (float)Math.PI / 2:(float)Math.PI*1.5f;
                                Projectile.frame = 0;
                                Projectile.rotation = 0;
                                break;
                            case 1:
                                
                                if (Projectile.frame != 1)
                                {
                                    Projectile.frame = 1;
                                    //damage enemies
                                    for (int i = 0; i < Main.maxNPCs; i++)
                                    {
                                        if (KingdomTerrahearts.instance.IsEnemy(i))
                                        {
                                            Main.npc[i].life -= Projectile.damage/2;
                                            Main.npc[i].checkDead();
                                        }
                                    }
                                    if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive())
                                    {
                                        Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(3, 15, 25).UseTargetPosition(Projectile.Center);
                                    }
                                }
                                if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                                {
                                    float progress = (100f - Projectile.ai[1]) / attackTimers[1]/2; // Will range from -3 to 3, 0 being the point where the bomb explodes.
                                    Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(150 * (1 - progress / 3f));
                                }
                                //stun enemies
                                for (int i = 0; i < Main.maxNPCs; i++)
                                {
                                    if (KingdomTerrahearts.instance.IsEnemy(i))
                                    {
                                        Main.npc[i].velocity = Vector2.Zero;

                                        Main.npc[i].life -= Projectile.damage / 5;
                                        Main.npc[i].checkDead();
                                    }
                                }

                                Projectile.velocity.X = 0;
                                break;
                            case 2:

                                if (Projectile.frame != 3)
                                {
                                    Projectile.frame = 3;
                                    Projectile.velocity.Y = -15;
                                }

                                Projectile.rotation += (float)Math.PI * 0.35f;

                                for(int i = 0; i < Main.rand.Next(0, 25);i++)
                                {
                                    Dust.NewDust(Projectile.position, (int)(Projectile.width), (int)(Projectile.height), DustID.Torch);
                                }

                                for (int i = 0; i < Main.maxNPCs; i++)
                                {
                                    if (KingdomTerrahearts.instance.IsEnemy(i))
                                    {
                                        Main.npc[i].AddBuff(BuffID.OnFire, 15);
                                    }
                                }

                                if (Projectile.velocity.Y > 0)
                                {
                                    for (int i = 0; i < Main.maxNPCs; i++)
                                    {
                                        if (KingdomTerrahearts.instance.IsEnemy(i))
                                        {
                                            Main.npc[i].life -= Projectile.damage / 10;
                                            Main.npc[i].checkDead();
                                        }
                                    }
                                }
                                else
                                {
                                    Projectile.velocity.Y +=  0.25f ;
                                }
                                break;
                        }
                    }
                    else
                    {
                        Projectile.velocity.X -= Projectile.velocity.X / Math.Abs(Projectile.velocity.X) * 0.5f;
                        Projectile.frame = 3;
                        Projectile.rotation += (float)(0.1f * Math.PI);
                    }
                }
                else
                {
                    if (Projectile.ai[0] != 0 || Projectile.ai[1]!=0)
                    {
                        if (Projectile.ai[0] == 1 && Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                        {
                            Filters.Scene["Shockwave"].Deactivate();
                        }
                        Projectile.ai[0] = Projectile.ai[1] = 0;
                    }
                    Projectile.frame = 2;
                    if (Math.Abs((Main.npc[target].Center - Projectile.Center).X) > Math.Abs((Main.npc[target].Center - Projectile.Center).Y))
                    {
                        Projectile.rotation = 0;
                        velX = (Main.npc[target].Center - Projectile.Center).X;
                        Projectile.velocity = new Vector2(velX / Math.Abs(velX) * 15, 0);
                    }
                    else
                    {
                        Projectile.rotation = (float)Math.PI/2;
                        velX = (Main.npc[target].Center - Projectile.Center).Y;
                        Projectile.velocity = new Vector2(0,velX / Math.Abs(velX) * 15);
                    }
                }

            }
            else
            {
                if (Projectile.ai[0] != 0 || Projectile.ai[1] != 0)
                {
                    if (Projectile.ai[0] == 1 && Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                    {
                        Filters.Scene["Shockwave"].Deactivate();
                    }
                    Projectile.ai[0] = Projectile.ai[1] = 0;
                }
                if (Vector2.Distance(Projectile.Center, Main.player[Projectile.owner].Center) < 300)
                {
                    if (Math.Abs(Projectile.velocity.X) < 1)
                    {
                        Projectile.frame = 0;
                        Projectile.velocity.X = 0;
                        Projectile.rotation = 0;
                        Projectile.velocity.Y += 0.25f;
                    }
                    else
                    {
                        Projectile.velocity.X -= Projectile.velocity.X / Math.Abs(Projectile.velocity.X)*0.5f;
                        Projectile.frame = 3;
                        Projectile.rotation += (float)(0.1f * Math.PI); 
                    }
                }
                else
                {
                    Projectile.frame = 2;
                    if (Math.Abs((Main.player[Projectile.owner].Center - Projectile.Center).X) > Math.Abs((Main.player[Projectile.owner].Center - Projectile.Center).Y))
                    {
                        velX = (Main.player[Projectile.owner].Center - Projectile.Center).X;
                        Projectile.velocity = new Vector2(velX / Math.Abs(velX) * 15, 0);
                    }
                    else
                    {
                        Projectile.rotation = (float)Math.PI / 2;
                        velX = (Main.player[Projectile.owner].Center - Projectile.Center).Y;
                        Projectile.velocity = new Vector2(0, velX / Math.Abs(velX) * 15);
                    }
                }
            }


            grounded = false;
        }

    }
}
