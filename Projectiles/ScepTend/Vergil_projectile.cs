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
    public class Vergil_projectile:ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 10;
            Projectile.height =24;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 150;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC || !target.friendly;
        }

        public override void Kill(int timeLeft)
        {
            ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);

            float rot = Main.rand.NextFloat((float)(-Math.PI * 2f), (float)(Math.PI * 2f));
            int proj = Projectile.NewProjectile(s,Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VergilSlice>(), Projectile.damage, 1);
            Main.projectile[proj].owner = Projectile.owner;
            Main.projectile[proj].rotation = rot;
            SoundEngine.PlaySound(SoundID.Item7.SoundId, x: (int)Main.projectile[proj].Center.X, y: (int)Main.projectile[proj].Center.Y, volumeScale: 3);
        }

        public override void AI()
        {
            if (MathHelp.Magnitude(Projectile.velocity) > 0)
                Projectile.velocity.Y += 0.1f;

            Projectile.rotation += MathHelp.Magnitude(Projectile.velocity) / 10;
        }

    }
}
