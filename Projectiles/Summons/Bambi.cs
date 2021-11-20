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
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 18;
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
            Projectile.velocity = Projectile.Center - Main.player[Projectile.owner].Center;
            Projectile.velocity.X = -Projectile.velocity.X / Math.Abs(Projectile.velocity.X)*5;
            Projectile.velocity.Y = -5;
            Projectile.spriteDirection = (Projectile.velocity.X > 0) ? 1 : -1;

            manaAmmount = Main.rand.Next(1, 5);
            for (int i = 0; i < manaAmmount; i++)
            {
                int item=Item.NewItem(Projectile.getRect(), ItemID.Star);
                manaDir = new Vector2(Main.rand.NextFloat(-5f, 5f),0);
                Main.item[item].velocity = manaDir;
                Projectile.ai[0]++;
            }


            return false;
        }

        public override void AI()
        {
            if (Projectile.ai[0] > 50)
            {
                Projectile.timeLeft = 1;
                return;
            }
            Player p = Main.player[Projectile.owner];

            Projectile.velocity.Y += 0.25f;

            lastPos = Projectile.Center;

            Projectile.frame = (Projectile.velocity.Y > 0) ? 1 : 0;

        }
    }
}
