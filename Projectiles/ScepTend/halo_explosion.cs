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
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 176;
            Projectile.height = 144;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.light = 1.5f;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.frame =(int)((1f-(Projectile.timeLeft / 30f))*5f);
        }

    }
}
