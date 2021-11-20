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
    public class Persona_projectile:ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 10;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 80;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void AI()
        {
            if (MathHelp.Magnitude(Projectile.velocity)>0)
            {
                Projectile.velocity.Y += 0.1f;
            }
        }

        public override void Kill(int timeLeft)
        {
            ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);

            int proj = Projectile.NewProjectile(s,Projectile.Center, Vector2.Zero, ModContent.ProjectileType<persona_fire>(), Projectile.damage * 4, Projectile.knockBack + 1,Projectile.owner);
            SoundEngine.PlaySound(SoundID.Item7.SoundId, x: (int)Projectile.Center.X, y: (int)Projectile.Center.Y, volumeScale: 3);
        }

    }
}
