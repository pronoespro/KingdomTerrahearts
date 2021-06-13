using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Wunderwaffe_explosion:ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 102;
            projectile.height = 102;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 30;
            projectile.scale = 0.75f;
            projectile.rotation = Main.rand.NextFloat((float)-Math.PI * 2, (float)Math.PI * 2);
            projectile.ai[0] = Main.rand.Next(1, 30);
        }

        public override void AI()
        {
            projectile.rotation += (float)(projectile.ai[0]/15f*Math.PI);
            projectile.velocity = Vector2.Zero;
            projectile.frame = (int)((1f - (projectile.timeLeft / 30f)) * 4f);
        }
    }
}
