using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Summons
{
    public class Mushu:SummonBase
    {
        Vector2 dir;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushu");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 18;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.scale = 1.5f;
            projectile.light = 1f;
            projectile.timeLeft = 2000;
            projectile.ai[1] = projectile.damage;
            projectile.damage = 0;
        }

        public override void AI()
        {
            projectile.Center = Main.player[projectile.owner].Center+new Vector2(0,-30);
            TargetEnemie();

            projectile.spriteDirection = Main.player[projectile.owner].direction;

            if (target != -1) {


                projectile.ai[0]++;
                if (projectile.ai[0] % 20 == 0)
                {
                    projectile.direction = (Main.npc[target].Center.X > projectile.Center.X) ? 1 : -1;
                    dir = MathHelp.Normalize(Main.npc[target].Center - projectile.Center)*15;
                    int proj=Projectile.NewProjectile(projectile.Center,dir, ProjectileID.BallofFire, (int)projectile.ai[1]+1, projectile.knockBack, projectile.owner);
                }
            }

        }

    }
}
