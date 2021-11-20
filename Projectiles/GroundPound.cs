using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;

namespace KingdomTerrahearts.Projectiles
{
    public class GroundPound:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("GroundPound");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {

            Projectile.netImportant = true;
            Projectile.width = 60;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 15;
        }

        public override void AI()
        {
            Projectile.scale = 3f;

            if (Projectile.timeLeft > 10)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Main.rand.Next(0, 3) <= 2)
                    {
                        int newDust=Dust.NewDust(Projectile.position, (int)(Projectile.width*1.5f), Projectile.height/3, DustID.Cloud,SpeedY:Main.rand.NextFloat(-3f,-1f),Alpha:Main.rand.Next(0,155),Scale:Main.rand.NextFloat(1f,2f));
                        Main.dust[newDust].noGravity=false;
                    }
                }
            }

            Projectile.frame = 3-Projectile.timeLeft / 5;
        }


    }
}
