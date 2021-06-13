using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class PersonaSlice : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 34;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 30;
            projectile.rotation = Main.rand.NextFloat((float)-Math.PI, (float)Math.PI);
        }

        public override void AI()
        {
            projectile.frame = (int)((projectile.timeLeft / 30f) * 8f);
        }
    }
}
