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
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 240/3;
            Projectile.scale =1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 0.75f;
            Projectile.timeLeft = 250/3;
            Projectile.ignoreWater = true;
            Projectile.rotation = 0;
        }

        public override void AI()
        {

            Projectile.ai[0]++;
            Projectile.ai[0] = (Projectile.ai[0]>=30)? 0:Projectile.ai[0];
            Projectile.frame = (int)(Projectile.ai[0] / 10);
            
            Projectile.alpha = 250-Projectile.timeLeft*2;
        }

    }
}
