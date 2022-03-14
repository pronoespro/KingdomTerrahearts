using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class shadowCloneProjectile:ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow Clone");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 60;
            Projectile.scale = 1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 100;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 100)
            {
                for(int i = 0; i < 5; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ash);
                }

                int closestPlayer = GetClosestPlayer();
                if(closestPlayer!=-1 && Main.player[closestPlayer].active && !Main.player[closestPlayer].dead)
                {
                    SoraPlayer sp = Main.player[closestPlayer].GetModPlayer<SoraPlayer>();
                    Projectile.frame = (sp.Grounded()) ? 0 : 1;
                }

            }

            if (Projectile.timeLeft < 50)
            {
                Projectile.damage = 75;
            }

            Projectile.alpha = (int)(MathF.Max((Projectile.timeLeft-50) / 50f * 255f,0));

            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ash,Alpha:Projectile.alpha);
            }

        }

        public int GetClosestPlayer()
        {
            int closest = -1;

            foreach(Player p in Main.player)
            {
                if(p!=null && p.active && !p.dead)
                {
                    closest =
                        (closest == -1 ||
                        Vector2.Distance(Main.player[closest].Center, Projectile.Center) < Vector2.Distance(p.Center, Projectile.Center)) ? p.whoAmI : closest;
                }
            }

            return closest;
        }

    }
}
