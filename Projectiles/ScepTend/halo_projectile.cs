using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class halo_projectile:ModProjectile
    {

        int target=-1;
        Vector2 targetLastPos;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 22;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            Projectile.scale = 0.5f;
            if (Projectile.ai[0] < 30)
            {
                Projectile.velocity.Y -= 0.1f;
                Projectile.velocity *= (MathHelp.Magnitude(Projectile.velocity) > 1) ? 0.9f : 1f;
                Projectile.ai[0]++;
            }
            else
            {
                if (CheckTarget())
                {
                    Projectile.velocity = MathHelp.Normalize(targetLastPos - Projectile.Center) * 7;
                    if (Vector2.Distance(targetLastPos, Projectile.Center) < Projectile.width)
                        Projectile.timeLeft = 1;
                }
                else
                {
                    Projectile.velocity *= (MathHelp.Magnitude(Projectile.velocity)>1)?0.9f:1f;
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
                            else if (Vector2.Distance(Projectile.Center, postarget.Center) < Vector2.Distance(Projectile.Center, Main.npc[target].Center))
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
            EntitySource_Parent s = new EntitySource_Parent(Projectile);

            int proj = Projectile.NewProjectile(s,Projectile.Center, Vector2.Zero, ModContent.ProjectileType<halo_explosion>(), Projectile.damage*4, Projectile.knockBack + 1,Projectile.owner);
            SoundEngine.PlaySound(SoundID.Item14,Projectile.Center);
        }

    }
}
