using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class halo_projectile:ModProjectile
    {

        int target=-1;
        Vector2 targetLastPos;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 22;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 120;
        }

        public override void AI()
        {
            projectile.scale = 0.5f;
            if (projectile.ai[0] < 30)
            {
                projectile.velocity.Y -= 0.1f;
                projectile.velocity *= (MathHelp.Magnitude(projectile.velocity) > 1) ? 0.9f : 1f;
                projectile.ai[0]++;
            }
            else
            {
                if (CheckTarget())
                {
                    projectile.velocity = MathHelp.Normalize(targetLastPos - projectile.Center) * 7;
                    if (Vector2.Distance(targetLastPos, projectile.Center) < projectile.width)
                        projectile.timeLeft = 1;
                }
                else
                {
                    projectile.velocity *= (MathHelp.Magnitude(projectile.velocity)>1)?0.9f:1f;
                }
            }
        }

        bool CheckTarget()
        {
            if (MathHelp.Magnitude(targetLastPos) == 0)
            {
                if (target == -1 || !Main.npc[target].active || Main.npc[target].friendly)
                {
                    target = -1;
                    NPC postarget;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        postarget = Main.npc[i];
                        if (postarget.active && !postarget.townNPC && !postarget.friendly && postarget.CanBeChasedBy(this))
                        {
                            if (target == -1)
                            {
                                target = i;
                                targetLastPos = postarget.Center;
                            }
                            else if (Vector2.Distance(projectile.Center, postarget.Center) < Vector2.Distance(projectile.Center, Main.npc[target].Center))
                            {
                                target = i;
                                targetLastPos = postarget.Center;
                            }
                        }
                    }
                }
            }
            return MathHelp.Magnitude(targetLastPos) != 0;
        }

        public override void Kill(int timeLeft)
        {
            int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("halo_explosion"), projectile.damage*4, projectile.knockBack + 1);
            Main.projectile[proj].owner = projectile.owner;
            Main.PlaySound(SoundID.Item14,projectile.Center);
        }

    }
}
