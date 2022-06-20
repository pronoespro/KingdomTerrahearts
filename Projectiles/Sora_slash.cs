using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class Sora_slash:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sora's Slash");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<KeybladeDamage>();
            Projectile.timeLeft = 10;
            Projectile.penetrate = -1;
            Projectile.light = 1;
        }

        public override void AI()
        {

            Projectile.alpha = (int)((10-Projectile.timeLeft) / 10f * 1);

            for(int i = 0; i < 15; i++)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TreasureSparkle);
                }
            }
        }

    }
}
