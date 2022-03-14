using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace KingdomTerrahearts.Projectiles
{
    public class VicennialProjectile:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gloden Experience");
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 15;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.light = 1;
        }

        public override void AI()
        {
            Projectile.scale = 3f;
            Projectile.frameCounter++;
            Projectile.frame = (int)(Projectile.frameCounter / 3f % 2f);

            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, -Projectile.velocity.X/2, -Projectile.velocity.Y/2);
            }
            Vector2 normalizedVel = MathHelp.Normalize(Projectile.velocity);
            Projectile.rotation = (float)Math.Atan2(normalizedVel.Y, normalizedVel.X);

            int target= Projectile.FindTargetWithLineOfSight();

            if(target>=0 && Main.npc[target].active && Vector2.Dot(Projectile.velocity,Main.npc[target].Center-Projectile.Center)>0.5f)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, MathHelp.Normalize(Main.npc[target].Center - Projectile.Center) * MathHelp.Magnitude(Projectile.velocity),0.1f);
            }

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Midas,60);
        }

        public override void Kill(int timeLeft)
        {
            int dustAmmount = Main.rand.Next(5, 30);
            for (int i = 0; i < dustAmmount; i++)
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.GoldCoin, (int)-Projectile.velocity.X/3,-Projectile.velocity.Y/3);

                float velRandomTang = (float)Math.Atan2(MathHelp.Normalize(-Projectile.velocity).Y, MathHelp.Normalize(-Projectile.velocity).X)+(float)Math.PI*Main.rand.NextFloat(-0.3f,0.3f);

                Vector2 desVel = new Vector2((float)Math.Cos(velRandomTang),(float)Math.Sin(velRandomTang))*MathHelp.Magnitude(Projectile.velocity)/2f;

                EntitySource_Parent s = new EntitySource_Parent(Projectile);
                Projectile.NewProjectile(s, Projectile.Center, desVel, ProjectileID.GoldCoin, Projectile.damage/10, 0, Projectile.owner);
            }
        }

    }
}
