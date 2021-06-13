using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Summons
{
    public class Bambi:ModProjectile
    {

        Vector2 lastPos;
        Vector2 manaDir;
        int manaAmmount;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bambi");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 18;
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
            projectile.velocity = projectile.Center - Main.player[projectile.owner].Center;
            projectile.velocity.X = -projectile.velocity.X / Math.Abs(projectile.velocity.X)*5;
            projectile.velocity.Y = -5;
            projectile.spriteDirection = (projectile.velocity.X > 0) ? 1 : -1;

            manaAmmount = Main.rand.Next(1, 5);
            for (int i = 0; i < manaAmmount; i++)
            {
                int item=Item.NewItem(projectile.getRect(), ItemID.Star);
                manaDir = new Vector2(Main.rand.NextFloat(-5f, 5f),0);
                Main.item[item].velocity = manaDir;
                projectile.ai[0]++;
            }


            return false;
        }

        public override void AI()
        {
            if (projectile.ai[0] > 50)
            {
                projectile.timeLeft = 1;
                return;
            }
            Player p = Main.player[projectile.owner];

            projectile.velocity.Y += 0.25f;

            lastPos = projectile.Center;

            projectile.frame = (projectile.velocity.Y > 0) ? 1 : 0;

        }
    }
}
