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

namespace KingdomTerrahearts.Projectiles.BossStuff
{
    public class sharpShooterProj:ModProjectile
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Xigbar/sharpshooter";

        public int bossOwner;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharpshooter");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 50;
            Projectile.timeLeft = 100;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            if (Projectile.ai[0] > 1)
            {
                Projectile.scale = 1.25f;
                Projectile.ai[0]--;
            }
        }

    }

    public class xigbar_missile : ModProjectile
    {

        public int projectileTarget = -1;
        public Vector2 targetPosition;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Missile");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            ResetProjectileTime();
        }

        public void ResetProjectileTime()
        {
            Projectile.timeLeft = 240 * 3;
        }

        public override void AI()
        {

            if (Projectile.hostile)
            {
                if (projectileTarget == -1 || !Main.player[projectileTarget].active || Main.player[projectileTarget].dead)
                {
                    FindClosestPlayer();
                }
            }
            else if (Projectile.friendly)
            {
                if (projectileTarget == -1 || !Main.npc[projectileTarget].active)
                {
                    projectileTarget = Projectile.FindTargetWithinRange(1200).whoAmI;
                }
            }

            GetTargetPosition();

            if (Projectile.timeLeft == 240 * 3)
            {
                //The stored speed of the projectile
                Projectile.ai[1] = 5;
                Projectile.ai[0] = MathHelp.Sign(Projectile.velocity.X);
            }
            else if (Projectile.timeLeft > 240 * 3 - 75)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.frame = 0;
            }
            else if (Projectile.timeLeft == 240 * 3 - 75)
            {
                Projectile.velocity = new Vector2(Projectile.ai[0] * Projectile.ai[1], -Projectile.ai[1]);
            }
            else
            {
                Projectile.frame = 1;
                if (Projectile.timeLeft % 60 == 0)
                {
                    Projectile.frame = 0;

                    if (Projectile.timeLeft % 240 == 120)
                    {
                        Projectile.velocity = MathHelp.Normalize(targetPosition - Projectile.Center) * Projectile.ai[1] * 2;
                    }
                    else
                    {
                        RandomizeDirection();
                    }
                }
            }
        }

        public void GetTargetPosition()
        {
            if (Projectile.hostile)
            {
                if (Main.player[projectileTarget].active || !Main.player[projectileTarget].dead)
                {
                    targetPosition = Main.player[projectileTarget].Center;
                }
                else
                {
                    FindClosestPlayer();
                }
            }
            else if (Projectile.friendly)
            {
                if (projectileTarget == -1 || !Main.npc[projectileTarget].active)
                {
                    projectileTarget = Projectile.FindTargetWithinRange(1200).whoAmI;
                }
                else
                {
                    targetPosition = Main.npc[projectileTarget].Center;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.timeLeft = 1;
        }

        public void RandomizeDirection()
        {
            Projectile.ai[0] *= -1;
            float randomDir = Main.rand.NextFloat(0, 1);
            Projectile.velocity = new Vector2(
                Projectile.ai[0] * Projectile.ai[1] * randomDir,
                MathHelp.Sign(-Projectile.velocity.Y) * Projectile.ai[1] * (1 - randomDir));
        }

        public void FindClosestPlayer(int maxDistance = -1)
        {
            int newTarget = -1;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && (maxDistance <= 0 || Vector2.Distance(Projectile.Center, Main.player[i].Center) < maxDistance))
                {

                    if (newTarget == -1 || Vector2.Distance(Main.player[i].Center, Projectile.Center) < Vector2.Distance(Main.player[newTarget].Center, Projectile.Center))
                    {
                        newTarget = i;
                    }

                }
            }

            projectileTarget = newTarget;
        }

    }


}
