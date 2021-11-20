using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class baseball:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baseball");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
        }

        public override void AI()
        {
            Projectile.velocity.Y+=0.25f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 360);
            crit = true;
        }

    }
}
