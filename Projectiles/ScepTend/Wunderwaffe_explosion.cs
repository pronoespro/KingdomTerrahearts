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
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 102;
            Projectile.height = 102;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.scale = 0.75f;
            Projectile.rotation = Main.rand.NextFloat((float)-Math.PI * 2, (float)Math.PI * 2);
            Projectile.ai[0] = Main.rand.Next(1, 30);
        }

        public override void AI()
        {
            Projectile.rotation += (float)(Projectile.ai[0]/15f*Math.PI);
            Projectile.velocity = Vector2.Zero;
            Projectile.frame = (int)((1f - (Projectile.timeLeft / 30f)) * 4f);
        }
    }
}
