using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Vergil_projectile:ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 10;
            projectile.height =24;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 150;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC || !target.friendly;
        }

        public override void Kill(int timeLeft)
        {
            float rot = Main.rand.NextFloat((float)(-Math.PI * 2f), (float)(Math.PI * 2f));
            int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("VergilSlice"), projectile.damage, 1);
            Main.projectile[proj].owner = projectile.owner;
            Main.projectile[proj].rotation = rot;
            Main.PlaySound(SoundID.Item7.SoundId, x: (int)Main.projectile[proj].Center.X, y: (int)Main.projectile[proj].Center.Y, volumeScale: 3);
        }

        public override void AI()
        {
            if (MathHelp.Magnitude(projectile.velocity) > 0)
                projectile.velocity.Y += 0.1f;

            projectile.rotation += MathHelp.Magnitude(projectile.velocity) / 10;
        }

    }
}
