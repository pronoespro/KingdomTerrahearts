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
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 156/3;
            Projectile.scale = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 1.2f;
            Projectile.timeLeft = 5;
            Projectile.ignoreWater = true;
            Projectile.rotation = 0;
            Projectile.alpha = 250;
        }

        public override void AI()
        {
            SoraPlayer sp = Main.player[Projectile.owner].GetModPlayer<SoraPlayer>();

            Projectile.position = Main.player[Projectile.owner].position-new Vector2(Projectile.width/4.6f,0);

            Projectile.ai[0]++;
            Projectile.ai[0] = (Projectile.ai[0] >= 30) ? 0 : Projectile.ai[0];
            Projectile.frame = (int)(Projectile.ai[0] / 10);

            Projectile.alpha = (sp.guardTime>15)?0: 150-sp.guardTime;
            Projectile.scale = 1.5f;
        }

    }
}
