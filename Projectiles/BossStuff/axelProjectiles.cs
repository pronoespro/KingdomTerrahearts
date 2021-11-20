using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using KingdomTerrahearts.Interface;

namespace KingdomTerrahearts.Projectiles.BossStuff
{

    public class AxelChakrams : ModProjectile
    {

        public override string Texture => "KingdomTerrahearts/Projectiles/Weapons/Chakram_EternalFlames";

        public int bossOwner;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chakram");
        }

        public override void SetDefaults()
        {
            Projectile.width =Projectile.height= 50;
            Projectile.timeLeft = 100;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            if (Main.rand.Next(2) == 0 && MathHelp.Magnitude(Projectile.velocity)>0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
            }
        }

    }


}
