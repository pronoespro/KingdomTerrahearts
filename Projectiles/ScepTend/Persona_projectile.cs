using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Persona_projectile:ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 10;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 80;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void AI()
        {
            if (MathHelp.Magnitude(projectile.velocity)>0)
            {
                projectile.velocity.Y += 0.1f;
            }
        }

        public override void Kill(int timeLeft)
        {
            int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("persona_fire"), projectile.damage * 4, projectile.knockBack + 1);
            Main.projectile[proj].owner = projectile.owner;
            Main.PlaySound(SoundID.Item7.SoundId, x: (int)projectile.Center.X, y: (int)projectile.Center.Y, volumeScale: 3);
        }

    }
}
