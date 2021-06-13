using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KingdomTerrahearts.Extra;
using Terraria.ModLoader.IO;
using System;

namespace KingdomTerrahearts.Projectiles.Weapons
{
    public class Chakram_EternalFlames:ChakramProjBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("EternalFlames");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height=30;
            projectile.timeLeft = 70;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 5;
            projectile.light = 0.5f;

            fireTime = 240;
        }

    }
}
