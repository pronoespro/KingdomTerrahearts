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
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 98;
            Projectile.height = 686/7;
            Projectile.scale = 0.75f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.frame = (int)((1f - (Projectile.timeLeft / 20f)) * 5f);
        }

    }
}
