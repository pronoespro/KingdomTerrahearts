using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class halo_dust:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 98;
            projectile.height = 686/7;
            projectile.scale = 0.75f;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 20;
        }

        public override void AI()
        {
            projectile.velocity = Vector2.Zero;
            projectile.frame = (int)((1f - (projectile.timeLeft / 20f)) * 5f);
        }

    }
}
