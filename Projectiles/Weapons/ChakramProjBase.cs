using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using KingdomTerrahearts.Extra;
using Terraria.ModLoader.IO;
using System;
using Microsoft.Xna.Framework;

namespace KingdomTerrahearts.Projectiles.Weapons
{
    public abstract class ChakramProjBase:ModProjectile
    {

        float initTimeLeft;
        float initVel;
        float initVelX;

        public Vector2 distToPlayer;
        public float gravity=1;
        public float rotOTime = 0.5f;
        public int fireTime = 120;
        public float damageMult = 0.5f;

        public int minProjectiles=2;
        public int maxProjectiles = 5;

        public int projectilesOnCollision= ProjectileID.BallofFire;
        public int projectileTrail = -1;
        public int trailTimer=5;
        public float trailSpeedMult=0.5f;
        public int manaUsage = 20;

        private int trail = 0;

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire,fireTime);
        }

        public override void AI()
        {


            initTimeLeft = (initTimeLeft > 0) ? initTimeLeft : Projectile.timeLeft;
            initVel = (initVel > 0) ? initVel : MathHelp.Magnitude(Projectile.velocity);
            initVelX = (initVel != 0) ? initVelX : Projectile.velocity.X;

            Projectile.spriteDirection = (initVel > 0) ? 1 : 0;
            Projectile.rotation += rotOTime*Projectile.spriteDirection;

            distToPlayer = Main.player[Projectile.owner].Center - Projectile.Center;

            if (Projectile.timeLeft < 5) return;


            if (Projectile.ai[0] < 50)
            {
                Projectile.velocity.Y -= gravity;
            }
            else
            {
                Projectile.velocity.Y += gravity;
                if (projectileTrail >= 0)
                {
                    trail++;
                    while (trail > trailTimer)
                    {
                        trail -= trailTimer;
                        if (Main.player[Projectile.owner].statMana > manaUsage)
                        {
                            EntitySource_Parent s = new EntitySource_Parent(Projectile);
                            int proj = Projectile.NewProjectile(s, Projectile.Center, Projectile.velocity * trailSpeedMult, projectileTrail, (int)(Math.Clamp(Projectile.damage * damageMult, 0, int.MaxValue)), 1, Projectile.owner);
                            Main.projectile[proj].friendly = true;
                            Main.projectile[proj].hostile = false;

                            Main.player[Projectile.owner].statMana -= manaUsage;
                            Main.player[Projectile.owner].manaRegenDelay= 5;

                        }
                        else
                        {
                            Projectile.ai[0] = 0;
                        }
                    }
                }
            }

            if (Projectile.timeLeft < 60)
            {
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, MathHelp.Normalize(distToPlayer) * initVel,0.2f);


                if (Vector2.Dot(MathHelp.Normalize(distToPlayer), MathHelp.Normalize(Projectile.velocity))==1)
                {
                    if (MathHelp.Magnitude(Main.player[Projectile.owner].velocity) > initVel)
                    {
                        Projectile.velocity = MathHelp.Normalize(Projectile.velocity) * MathHelp.Magnitude(Main.player[Projectile.owner].velocity) * 2;
                        Projectile.Center = (MathHelp.Magnitude(distToPlayer) > 500) ? Main.player[Projectile.owner].Center + MathHelp.Normalize(distToPlayer) * 500 : Projectile.Center;
                    }

                }
            }
            if (Projectile.timeLeft < 40)
                Projectile.timeLeft = (MathHelp.Magnitude(distToPlayer) > MathHelp.Magnitude(Projectile.velocity) * 5) ? 50 : 1;

            Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Torch);

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.timeLeft = 40;

            if (projectilesOnCollision>=0 && Projectile.ai[0] > 50 && Main.player[Projectile.owner].statMana>manaUsage)
            {
                EntitySource_Parent s = new EntitySource_Parent(Projectile);
                int projAmmount = Main.rand.Next(minProjectiles,maxProjectiles);
                for (int i = 0; i < projAmmount; i++)
                {
                    int proj=Projectile.NewProjectile(s,Projectile.Center, new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-5, -2)), projectilesOnCollision, (int)(Math.Clamp(Projectile.damage * damageMult,0,int.MaxValue)), 1,Projectile.owner);
                    Main.projectile[proj].friendly = true;
                    Main.projectile[proj].hostile= false;
                }

                Main.player[Projectile.owner].statMana -= manaUsage;
            }

            return false;
        }

    }
}
