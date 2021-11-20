using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

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
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 192 / 4;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 0.75f;
            Projectile.light = 1f;
            Projectile.timeLeft = 3000;
        }

        public override void AI()
        {
            Projectile.frame = 0;
            if (Projectile.damage > 0)
            {
                Projectile.ai[1] = Projectile.damage;
                Projectile.damage = 0;
            }

            if (Projectile.timeLeft >= 2999)
            {
                owner = Main.player[Projectile.owner];
            }

            TargetEnemie();

            if (owner != null)
            {
                Projectile.velocity.Y = (owner.Center.Y < Projectile.Center.Y - 25) ? -5 : Projectile.velocity.Y + 0.25f;
                Projectile.velocity.X = (Math.Abs(owner.Center.X - Projectile.Center.X) < 200) ? 0 : MathHelp.Sign(owner.Center.X - Projectile.Center.X) * 5;

                Projectile.frameCounter += (int)Projectile.velocity.X;
                Projectile.frame = (((Projectile.frameCounter / 25) % 2) == 0) ? 0 : 3;

                if (owner.statLife < owner.statLifeMax * 0.4f)
                {
                    ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);
                    owner.statLife += (int)(owner.statLifeMax * 0.4f);
                    Projectile.timeLeft -= 500;
                    Projectile.frame = 1;

                    int proj = Projectile.NewProjectile(s, Projectile.Center, new Vector2(0, -2), ProjectileID.HallowStar, 0, 0, Projectile.owner);
                    Main.projectile[proj].timeLeft = 50;
                }
                else
                {
                    if (target != -1 && Main.npc[target].active)
                    {
                        Projectile.ai[0]++;

                        if (Projectile.ai[0] > attackTimers[curAttack])
                        {
                            curAttack = (curAttack == 0) ? Main.rand.Next(1, 4) : 0;
                            Projectile.ai[0] = 0;
                            if (curAttack == 1 || curAttack == 2)
                            {
                                Projectile.velocity.Y = -5;
                            }
                            if (curAttack == 1)
                            {
                                Projectile.timeLeft -= 150;
                            }
                        }
                        switch (curAttack)
                        {
                            case 0:
                                break;
                            case 1:
                                Whisle();
                                NoFall();
                                Projectile.frame = 2;
                                break;
                            case 2:
                                Projectile.frame = 3;
                                if (Projectile.ai[0] % 10 == 0)
                                {
                                    ThrowBall(new Vector2(Main.rand.Next(-7, 7), 0), 7);
                                }
                                NoFall();
                                break;
                            case 3:
                                Projectile.frame = 2;
                                if (Projectile.ai[0] % 15 == 0)
                                {
                                    ThrowBall(Main.npc[target].Center - Projectile.Center, 7, 2.5f);
                                }
                                NoFall();
                                break;
                        }
                    }
                }

                Projectile.tileCollide = (Vector2.Distance(Projectile.Center, owner.Center) < 700);

                if (Math.Abs(Projectile.velocity.X) > 1)
                {
                    Projectile.spriteDirection = (int)MathHelp.Sign(Projectile.velocity.X);
                }
            }
            Projectile.hide = Projectile.velocity.Y > 0;

        }
        
        void NoFall()
        {
            Projectile.velocity.Y = (Projectile.velocity.Y > 0) ? 0 : Projectile.velocity.Y;
        }

        void Whisle()
        {
            NPC whisledNpc;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                whisledNpc = Main.npc[i];
                if(((whisledNpc.damage > 0 && !whisledNpc.friendly) || whisledNpc.boss) && whisledNpc.active && Vector2.Distance(whisledNpc.Center,Projectile.Center)<500)
                {
                    whisledNpc.velocity = (Vector2.Distance(whisledNpc.Center, Projectile.Center) < 50) ? Vector2.Zero : (MathHelp.Normalize(Projectile.Center - whisledNpc.Center) * 15);
                    whisledNpc.life -=Projectile.damage/10+1;
                    whisledNpc.checkDead();
                }
            }
        }

        void ThrowBall(Vector2 velocity,float speed,float damageMult=1)
        {
            ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);

            int proj = Projectile.NewProjectile(s,Projectile.Center, MathHelp.Normalize(velocity)*speed, ModContent.ProjectileType<baseball>(), (int)((Projectile.ai[1]+1)*damageMult), Projectile.knockBack,Projectile.owner);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y = 0;
            return false;
        }

    }
}
