using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class guardProjectile:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("guardProjectile");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 52;
            projectile.height = 156/3;
            projectile.scale = 2;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.light = 1.2f;
            projectile.timeLeft = 5;
            projectile.ignoreWater = true;
            projectile.rotation = 0;
            projectile.alpha = 250;
        }

        public override void AI()
        {
            SoraPlayer sp = Main.player[projectile.owner].GetModPlayer<SoraPlayer>();

            projectile.position = Main.player[projectile.owner].position-new Vector2(projectile.width/4.6f,0);

            projectile.ai[0]++;
            projectile.ai[0] = (projectile.ai[0] >= 30) ? 0 : projectile.ai[0];
            projectile.frame = (int)(projectile.ai[0] / 10);

            projectile.alpha = (sp.guardTime>15)?0: 150-sp.guardTime;
            projectile.scale = 1.5f;
        }

    }
}
