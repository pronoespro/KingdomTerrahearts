using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class roxasLightBeam : ModProjectile
    {

        float timeLeft = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Beam");
        }

        public override void SetDefaults()
        {

            Projectile.width = 30;
            Projectile.height = 120;
            Projectile.aiStyle = -1;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.light = 1.5f;
            Projectile.maxPenetrate = -1;
            Projectile.DamageType= ModContent.GetInstance<KeybladeDamage>();
            Projectile.penetrate = 1000;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 75;

        }

        public override void AI()
        {
            Projectile.velocity.X = Projectile.velocity.X + Projectile.velocity.Y*MathHelp.Sign(Projectile.velocity.X);
            Projectile.velocity.Y = 0;

            timeLeft = (timeLeft == 0) ? Projectile.timeLeft : timeLeft;

            Projectile.alpha = (Projectile.timeLeft > 50) ? 100 : 100 + 155 * (Projectile.timeLeft);
        }

    }
}
