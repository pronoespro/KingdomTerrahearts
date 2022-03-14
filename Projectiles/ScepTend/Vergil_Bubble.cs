using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Vergil_Bubble:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.velocity = Vector2.Zero;
            Projectile.damage = 0;
            Projectile.ai[0] = (Projectile.ai[0] == 0) ? 1 : Projectile.ai[0];
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC || !target.friendly;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 30)
            {
                SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            }

            foreach(Player p in Main.player)
            {
                if (p.active)
                {
                    SoraPlayer s = p.GetModPlayer<SoraPlayer>();
                    s.ModifyCutsceneCamera(Projectile.Center - p.Center, 0.75f, 2, 10, 100);
                }
            }

            Projectile.alpha = (int)(250 - (Projectile.timeLeft / 30f)*5);
            Projectile.damage = 0;
            Projectile.scale =(3-(1-(Projectile.timeLeft/30f)/5))*Projectile.ai[0];
            Projectile.frame = (int)((1f - (Projectile.timeLeft / 30f)) * 4f);

            for (int i = 0; i < 15; i++)
            {
                if (Main.rand.Next(3) == 0)
                {
                    Vector2 scale = new Vector2(Projectile.width * Projectile.scale, Projectile.height * Projectile.scale);
                    Dust.NewDust(Projectile.Center - scale / 4, (int)scale.X, (int)scale.Y, DustID.Electric);
                }
            }

        }

    }
}
