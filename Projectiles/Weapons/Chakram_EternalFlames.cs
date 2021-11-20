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
            Projectile.width = Projectile.height=30;
            Projectile.timeLeft = 70;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.light = 0.5f;

            fireTime = 240;
        }

    }
}
