using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire,fireTime);
        }

        public override void AI()
        {
            initTimeLeft = (initTimeLeft > 0) ? initTimeLeft : projectile.timeLeft;
            initVel = (initVel > 0) ? initVel : MathHelp.Magnitude(projectile.velocity);
            initVelX = (initVel != 0) ? initVelX : projectile.velocity.X;

            projectile.spriteDirection = (initVel > 0) ? 1 : 0;
            projectile.rotation += rotOTime*projectile.spriteDirection;

            distToPlayer = Main.player[projectile.owner].Center - projectile.Center;

            if (projectile.timeLeft < 5) return;


            if (projectile.ai[0] < 50)
            {
                projectile.velocity.Y -= gravity;
            }
            else
            {
                projectile.velocity.Y += gravity;
            }

            if (projectile.timeLeft < 60)
            {
                projectile.tileCollide = false;
                projectile.velocity = Vector2.Lerp(projectile.velocity, MathHelp.Normalize(distToPlayer) * initVel,0.2f);


                if (Vector2.Dot(MathHelp.Normalize(distToPlayer), MathHelp.Normalize(projectile.velocity))==1)
                {
                    if (MathHelp.Magnitude(Main.player[projectile.owner].velocity) > initVel / 2)
                    {
                        projectile.velocity = MathHelp.Normalize(projectile.velocity) * MathHelp.Magnitude(Main.player[projectile.owner].velocity) * 2;
                        projectile.Center = (MathHelp.Magnitude(distToPlayer) > 500) ? Main.player[projectile.owner].Center + MathHelp.Normalize(distToPlayer) * 500 : projectile.Center;
                    }

                }
            }
            if (projectile.timeLeft < 40)
                projectile.timeLeft = (MathHelp.Magnitude(distToPlayer) > MathHelp.Magnitude(projectile.velocity) * 5) ? 50 : 1;

            Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Fire);

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.timeLeft = 40;

            if (projectile.ai[0] > 50 && Main.player[projectile.owner].statMana>20)
            {
                int projAmmount = Main.rand.Next(2,5);
                for (int i = 0; i < projAmmount; i++)
                {
                    Projectile.NewProjectile(projectile.Center, new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(2, 5)), ProjectileID.BallofFire, projectile.damage / 2, 1);
                }

                Main.player[projectile.owner].statMana -= 20;
            }

            return false;
        }

    }
}
