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
    public class guardExpandProjectile:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic counter");
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.alpha = 100;
            Projectile.penetrate = 5;
            Projectile.knockBack = 10;
            Projectile.timeLeft = 50;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.scale += 0.1f*(Projectile.timeLeft/100f);
            Projectile.alpha += (int)(15 * (Projectile.timeLeft / 50f));
        }

    }
}
