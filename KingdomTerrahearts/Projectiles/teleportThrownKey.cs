using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    class teleportThrownKey:ModProjectile
    {

        int projTimeLeft;
        int projInitTP = 55;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thrown Keyblade");
        }
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.scale = 1f;
            projectile.damage = 0;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.melee = true;
            projectile.timeLeft = 60;
            projTimeLeft = projectile.timeLeft;
        }

        public override void AI()
        {

            projTimeLeft--;
            projectile.rotation += 1;

            float light = 4f * projectile.scale;
            Lighting.AddLight(projectile.Center, light, light, light);

            if (projectile.frameCounter%10==0)
            {
                int dust=Dust.NewDust(projectile.Center, projectile.width, projectile.height, 70);
                //ModDust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, mod, "Sparkle", projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }

        public override void Kill(int timeLeft)
		{
            if (projectile.timeLeft == 0 )
            {
                if (projTimeLeft > 0 && projTimeLeft <= projInitTP)
                {
                    if (projectile.owner == Main.myPlayer)
                    {
                        Player player = Main.player[projectile.owner];
                        SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                        sp.tpFallImmunity = 5;
                        player.Center = projectile.position - Vector2.Normalize(projectile.velocity) * player.height / 2;
                        player.velocity = new Vector2();

                        //Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Item, "Sounds/Item/BubblinePopSound"));
                    }
                }
                else
                {
                    for (int i = 0; i < 27; i++)
                    {
                        float xRand = Main.rand.NextFloat(-3, 3);
                        float yRand = Main.rand.NextFloat(-3, 3);
                        int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 70,xRand,yRand);
                    }
                }
            }
		}


	}
}
