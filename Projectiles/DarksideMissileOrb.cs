using KingdomTerrahearts.NPCs.Bosses;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    class DarksideMissileOrb:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark orb");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
        }

        public override void AI()
        {

            if (++Projectile.frameCounter >= 5)
            {
                Vector2 randRot= new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                randRot.Normalize();
                while (randRot.X == 0 && randRot.Y == 0)
                {
                    randRot = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)); ;
                    randRot.Normalize();
                }

                int dust = Dust.NewDust(Projectile.Center + randRot, Projectile.width, Projectile.height, 200, randRot.X, randRot.Y, 125);
                Main.dust[dust].color = Color.Black;
                Main.dust[dust].noGravity = true;

                Projectile.frameCounter = 0;
                Projectile.rotation += Main.rand.Next(-90, 90);
                if (++Projectile.frame >= 3)
                {
                    Projectile.frame = 0;
                }
            }

        }

    }
}
