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
            }

            if (Projectile.timeLeft < 60)
            {
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, MathHelp.Normalize(distToPlayer) * initVel,0.2f);


                if (Vector2.Dot(MathHelp.Normalize(distToPlayer), MathHelp.Normalize(Projectile.velocity))==1)
                {
                    if (MathHelp.Magnitude(Main.player[Projectile.owner].velocity) > initVel / 2)
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

            if (Projectile.ai[0] > 50 && Main.player[Projectile.owner].statMana>20)
            {
                ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);
                int projAmmount = Main.rand.Next(2,5);
                for (int i = 0; i < projAmmount; i++)
                {
                    int proj=Projectile.NewProjectile(s,Projectile.Center, new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(2, 5)), ProjectileID.BallofFire, Projectile.damage / 2, 1,Projectile.owner);
                }

                Main.player[Projectile.owner].statMana -= 20;
            }

            return false;
        }

    }
}
