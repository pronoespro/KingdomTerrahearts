using KingdomTerrahearts.Extra;
using KingdomTerrahearts.NPCs.Bosses;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    class EnemyKingdomKey:ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Keyblade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.scale = 0.5f;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 0.75f;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelp.DegreeToQuat(90);
        }

    }
}
