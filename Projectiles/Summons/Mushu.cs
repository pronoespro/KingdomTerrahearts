using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace KingdomTerrahearts.Projectiles.Summons
{
    public class Mushu:SummonBase
    {
        Vector2 dir;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushu");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1.5f;
            Projectile.light = 1f;
            Projectile.timeLeft = 2000;
            Projectile.ai[1] = Projectile.damage;
            Projectile.damage = 0;
        }

        public override void AI()
        {
            Projectile.Center = Main.player[Projectile.owner].Center+new Vector2(0,-30);
            TargetEnemie();

            Projectile.spriteDirection = Main.player[Projectile.owner].direction;

            if (target != -1) {


                Projectile.ai[0]++;
                if (Projectile.ai[0] % 20 == 0)
                {
                    EntitySource_Parent s = new EntitySource_Parent(Projectile);

                    Projectile.direction = (Main.npc[target].Center.X > Projectile.Center.X) ? 1 : -1;
                    dir = MathHelp.Normalize(Main.npc[target].Center - Projectile.Center)*15;
                    int proj=Projectile.NewProjectile(s,Projectile.Center,dir, ProjectileID.BallofFire, (int)Projectile.ai[1]+1, Projectile.knockBack, Projectile.owner);
                }
            }

        }

    }
}
