using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class halo_explosion:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 176;
            projectile.height = 144;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 30;
            projectile.light = 1.5f;
        }

        public override void AI()
        {
            projectile.velocity = Vector2.Zero;
            projectile.frame =(int)((1f-(projectile.timeLeft / 30f))*5f);
        }

    }
}
