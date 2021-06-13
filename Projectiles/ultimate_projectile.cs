using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class ultimate_projectile:ModProjectile
    {

        public float offset=0;
        public Vector2 distToPlayer;
        public float yvel;
        public float initTime;
        public float spinVel = 0.1f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultimate sword");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 24;
            projectile.aiStyle = -1;
            projectile.alpha = 100;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.light = 1.5f;
            projectile.maxPenetrate = -1;
            projectile.melee = true;
            projectile.penetrate = 1000;
            projectile.tileCollide = false;
            projectile.timeLeft = 50;
            projectile.frame = Main.rand.Next(5);

        }

        public override void AI()
        {
            if (projectile.timeLeft > 49)
            {
                yvel = projectile.Center.Y - Main.player[projectile.owner].Center.Y;
                distToPlayer = projectile.Center - Main.player[projectile.owner].Center;
                initTime = projectile.ai[0];
                projectile.ai[0] = 55;
            }
            if (projectile.ai[0] == 0) projectile.ai[0] = projectile.timeLeft;
            if (projectile.ai[1] == 0) projectile.ai[1] = projectile.velocity.X;

            Main.player[projectile.owner].velocity = new Vector2(projectile.ai[1] * (projectile.timeLeft / projectile.ai[0]),0);
            Main.player[projectile.owner].gravity = 0;

            yvel++;
            offset =  (((float)Math.Sin(initTime+yvel* spinVel)*2f)-1) * Math.Abs(distToPlayer.Y);

            Main.player[projectile.owner].heldProj = projectile.whoAmI;
            Main.player[projectile.owner].itemLocation = Main.player[projectile.owner].Center;
            Main.player[projectile.owner].itemTime = Main.player[projectile.owner].itemAnimation = 2;
            Main.player[projectile.owner].itemRotation = 0;

            projectile.Center = Main.player[projectile.owner].Center+new Vector2(distToPlayer.X,offset);

            projectile.spriteDirection = projectile.direction = Main.player[projectile.owner].direction;
        }

    }
}
