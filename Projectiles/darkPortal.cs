using KingdomTerrahearts.Extra;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class darkPortal:ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Keyblade");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 240/3;
            projectile.scale =1;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.light = 0.75f;
            projectile.timeLeft = 250/3;
            projectile.ignoreWater = true;
            projectile.rotation = 0;
        }

        public override void AI()
        {

            projectile.ai[0]++;
            projectile.ai[0] = (projectile.ai[0]>=30)? 0:projectile.ai[0];
            projectile.frame = (int)(projectile.ai[0] / 10);
            
            projectile.alpha = 250-projectile.timeLeft*2;
        }

    }
}
