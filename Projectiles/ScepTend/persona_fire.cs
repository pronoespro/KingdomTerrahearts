using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class persona_fire:ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 34;
            Projectile.height = 90;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
        }

        public override void AI()
        {
            ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);

            if (Projectile.ai[0] == 0)
            {
                SpawnProjectile(ModContent.ProjectileType<Persona_ground>(),offsetY:5);
                Projectile.Center -= new Vector2(0, Projectile.height / 3);
                Projectile.ai[0] = 1;
            }

            if (Projectile.timeLeft > 20)
            {
                if (Projectile.timeLeft % 45 == 0)
                {
                    SpawnProjectile(ModContent.ProjectileType<persona_explosion>(), offsetY: -25 + Projectile.height / 3);
                }

                if (Projectile.timeLeft % 15 == 0)
                {
                    SpawnProjectile(ModContent.ProjectileType<PersonaSlice>(),offsetX:Main.rand.NextFloat(-5,5), offsetY: 20 + Main.rand.NextFloat(0, 5));
                }
            }

            Projectile.alpha = (Projectile.timeLeft > 80) ? (int)(((Projectile.timeLeft - 80f) / 20f)*250) : (int)(250f*(1f-Projectile.timeLeft/100f));
            Projectile.velocity = Vector2.Zero;
        }

        public int SpawnProjectile(int type, float offsetX = 0, float offsetY = 0)
        {
            ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);

            Vector2 offset = new Vector2(offsetX, offsetY);
            int proj = Projectile.NewProjectile(s,Projectile.Center+offset, Vector2.Zero,type,0,Projectile.knockBack,Projectile.owner);
            return proj;
        }

    }
}
