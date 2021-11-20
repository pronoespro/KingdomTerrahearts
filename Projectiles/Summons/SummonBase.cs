using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Summons
{
    public abstract class SummonBase:ModProjectile
    {
        public int target = -1;

        public void TargetEnemie()
        {
            target = (target == -1 || !Main.npc[target].active) ? -1 : target;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (KingdomTerrahearts.instance.IsEnemy(i))
                {
                    if (target == -1 || Vector2.Distance(Projectile.Center, Main.npc[i].Center) < Vector2.Distance(Projectile.Center, Main.npc[target].Center))
                    {
                        target = i;
                    }
                }
            }
        }
    }
}
