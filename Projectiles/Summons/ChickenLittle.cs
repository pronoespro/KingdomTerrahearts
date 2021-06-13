using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Summons
{
    public class ChickenLittle : SummonBase
    {

        int curAttack = 0;

        int[] attackTimers = new int[] { 125, 200, 50, 25 };
        Player owner;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chicken Little");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 192 / 4;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.scale = 0.75f;
            projectile.light = 1f;
            projectile.timeLeft = 3000;
        }

        public override void AI()
        {
            if (projectile.damage > 0)
            {
                projectile.ai[1] = projectile.damage;
                projectile.damage = 0;
            }

            if (projectile.timeLeft>=2999)
                owner = Main.player[projectile.owner];

            TargetEnemie();

            projectile.velocity.Y = (owner.Center.Y < projectile.Center.Y - 25) ? -5 : projectile.velocity.Y + 0.25f;
            projectile.velocity.X = (Math.Abs(owner.Center.X - projectile.Center.X) < 200) ? 0 : MathHelp.Sign(owner.Center.X - projectile.Center.X) * 5;

            projectile.frameCounter += (int)projectile.velocity.X;
            projectile.frame = (((projectile.frameCounter / 25) % 2)==0)?0:3;

            if (owner.statLife < owner.statLifeMax * 0.4f)
            {
                owner.statLife += (int)(owner.statLifeMax * 0.4f);
                projectile.timeLeft -= 500;
                projectile.frame = 1;

                int proj = Projectile.NewProjectile(projectile.Center, new Vector2(0, -2), ProjectileID.HallowStar,0,0,projectile.owner);
                Main.projectile[proj].timeLeft = 50;
            }
            else
            {
                if (target != -1 && Main.npc[target].active)
                {
                    projectile.ai[0]++;

                    if (projectile.ai[0] > attackTimers[curAttack])
                    {
                        curAttack = (curAttack == 0) ? Main.rand.Next(1, 4) : 0;
                        projectile.ai[0] = 0;
                        if (curAttack == 1 || curAttack == 2)
                        {
                            projectile.velocity.Y = -5;
                        }
                        if (curAttack == 1)
                        {
                            projectile.timeLeft -= 150;
                        }
                    }
                    switch (curAttack)
                    {
                        case 0:
                            break;
                        case 1:
                            Whisle();
                            NoFall();
                            projectile.frame = 2;
                            break;
                        case 2:
                            projectile.frame = 3;
                            if (projectile.ai[0] % 10 == 0)
                            {
                                ThrowBall(new Vector2(Main.rand.Next(-7, 7), 0), 7);
                            }
                            NoFall();
                            break;
                        case 3:
                            projectile.frame = 2;
                            if (projectile.ai[0] % 15 == 0)
                            {
                                ThrowBall(Main.npc[target].Center - projectile.Center, 7,2.5f);
                            }
                            NoFall();
                            break;
                    }
                }
            }

            projectile.tileCollide = (Vector2.Distance(projectile.Center, owner.Center) < 700);

            if(Math.Abs(projectile.velocity.X)>1)
                projectile.spriteDirection = (int)MathHelp.Sign(projectile.velocity.X);

        }
        
        void NoFall()
        {
            projectile.velocity.Y = (projectile.velocity.Y > 0) ? 0 : projectile.velocity.Y;
        }

        void Whisle()
        {
            NPC whisledNpc;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                whisledNpc = Main.npc[i];
                if(((whisledNpc.damage > 0 && !whisledNpc.friendly) || whisledNpc.boss) && whisledNpc.active)
                {
                    whisledNpc.velocity = (Vector2.Distance(whisledNpc.Center, projectile.Center) < 50) ? Vector2.Zero : (MathHelp.Normalize(projectile.Center - whisledNpc.Center) * 15);
                    whisledNpc.life -=projectile.damage/10+1;
                    whisledNpc.checkDead();
                }
            }
        }

        void ThrowBall(Vector2 velocity,float speed,float damageMult=1)
        {
            int proj = Projectile.NewProjectile(projectile.Center, MathHelp.Normalize(velocity)*speed, mod.ProjectileType("baseball"), (int)((projectile.ai[1]+1)*damageMult), projectile.knockBack,projectile.owner);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity.Y = 0;
            return false;
        }

    }
}
