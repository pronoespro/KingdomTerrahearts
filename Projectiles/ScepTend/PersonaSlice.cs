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
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 34;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.rotation = Main.rand.NextFloat((float)-Math.PI, (float)Math.PI);
        }

        public override void AI()
        {
            Projectile.frame = (int)((Projectile.timeLeft / 30f) * 8f);
        }
    }
}
