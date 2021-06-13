using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class persona_fire:ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 34;
            projectile.height = 90;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 100;
        }

        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                SpawnProjectile(mod.ProjectileType("Persona_ground"),offsetY:5);
                projectile.Center -= new Vector2(0, projectile.height / 3);
                projectile.ai[0] = 1;
            }

            if (projectile.timeLeft > 20)
            {
                if (projectile.timeLeft % 45 == 0)
                {
                    SpawnProjectile(mod.ProjectileType("persona_explosion"), offsetY: -25 + projectile.height / 3);
                }

                if (projectile.timeLeft % 15 == 0)
                {
                    SpawnProjectile(mod.ProjectileType("PersonaSlice"),offsetX:Main.rand.NextFloat(-5,5), offsetY: 20 + Main.rand.NextFloat(0, 5));
                }
            }

            projectile.alpha = (projectile.timeLeft > 80) ? (int)(((projectile.timeLeft - 80f) / 20f)*250) : (int)(250f*(1f-projectile.timeLeft/100f));
            projectile.velocity = Vector2.Zero;
        }

        public int SpawnProjectile(int type, float offsetX = 0, float offsetY = 0)
        {
            Vector2 offset = new Vector2(offsetX, offsetY);
            int proj = Projectile.NewProjectile(projectile.Center+offset, Vector2.Zero,type,0,projectile.knockBack);
            Main.projectile[proj].owner = projectile.owner;
            return proj;
        }

    }
}
