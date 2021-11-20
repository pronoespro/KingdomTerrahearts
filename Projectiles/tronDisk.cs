using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    class tronDisk:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Disk");
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.scale = 0.5f;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<KeybladeDamage>();
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
        }

    }
}
