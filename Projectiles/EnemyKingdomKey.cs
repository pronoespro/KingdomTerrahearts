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
            projectile.width = 80;
            projectile.height = 80;
            projectile.scale = 0.5f;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.light = 0.75f;
        }

        public override void AI()
        {
            projectile.rotation += MathHelp.DegreeToQuat(90);
        }

    }
}
