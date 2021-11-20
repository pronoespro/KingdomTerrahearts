using KingdomTerrahearts.Extra;
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
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.scale = 1f;
            Projectile.damage = 0;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<KeybladeDamage>();
            Projectile.timeLeft = 60;
            projTimeLeft = Projectile.timeLeft;
        }

        public override void AI()
        {

            projTimeLeft--;
            Projectile.rotation += 1;

            float light = 4f * Projectile.scale;
            Lighting.AddLight(Projectile.Center, light, light, light);

            if (Projectile.frameCounter%10==0)
            {
                int dust=Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, 70);
                //ModDust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, mod, "Sparkle", Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void Kill(int timeLeft)
		{
            if (Projectile.timeLeft == 0 )
            {
                if (projTimeLeft > 0 && projTimeLeft <= projInitTP)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Player player = Main.player[Projectile.owner];
                        SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                        sp.tpFallImmunity = 5;
                        player.Center = Projectile.position - Vector2.Normalize(Projectile.velocity) * player.height / 2;
                        player.velocity = new Vector2();

                        //Main.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, mod.GetSoundSlot(SoundType.Item, "Sounds/Item/BubblinePopSound"));
                    }
                }
                else
                {
                    for (int i = 0; i < 27; i++)
                    {
                        float xRand = Main.rand.NextFloat(-3, 3);
                        float yRand = Main.rand.NextFloat(-3, 3);
                        int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, 70,xRand,yRand);
                    }
                }
            }
		}


	}
}
