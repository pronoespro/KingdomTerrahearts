using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Magic
{
    public class magnet:ModProjectile
    {

        NPC magnetizedNpc;

        int orbRot;
        float orbPos;
        Vector2 curOrbPos;
        bool blueInFront;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnet");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 100;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 100000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 300)
            {
                Projectile.Center += new Vector2(0, -100);
            }

            orbRot++;

            orbPos = (float)(Math.Sin(orbRot/10f) * 40f);

            blueInFront = orbRot % 20 > 5 && orbRot % 20 < 15;

            Projectile.frameCounter++;
            Projectile.frame = (Projectile.frameCounter / 7) % 3;

            if (Projectile.damage > 0)
            {
                Projectile.ai[1] = Projectile.damage;
                Projectile.damage = 0;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                magnetizedNpc = Main.npc[i];
                if (((magnetizedNpc.damage > 0 && !magnetizedNpc.friendly)) && magnetizedNpc.active)
                {
                    magnetizedNpc.velocity = (Vector2.Distance(magnetizedNpc.Center, Projectile.Center) < 55) ? Vector2.Zero : (MathHelp.Normalize(Projectile.Center - magnetizedNpc.Center) * 25);

                    magnetizedNpc.life -= (int)Math.Max(0f, ((Projectile.ai[1]+1) / 10f + 1f)-Vector2.Distance(Projectile.Center,magnetizedNpc.Center)/55f);

                    magnetizedNpc.checkDead();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!blueInFront)
            {
                curOrbPos = Projectile.Center - Main.screenPosition + new Vector2(orbPos, 0);

                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/Magic/magnetOrb_blue"), curOrbPos, new Rectangle(0, 0, 20, 20), lightColor, 0f, new Vector2(10, 10), 1, SpriteEffects.None, 0f);
            }
            else
            {
                curOrbPos = Projectile.Center - Main.screenPosition + new Vector2(-orbPos, 0);

                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/Magic/magnetOrb_orange"), curOrbPos, new Rectangle(0, 0, 20, 20), lightColor, 0f, new Vector2(10, 10), 1, SpriteEffects.None, 0f);
            }
            return base.PreDraw(ref lightColor);
        }

        public override void PostDraw(Color lightColor)
        {
            if (blueInFront)
            {
                curOrbPos = Projectile.Center - Main.screenPosition + new Vector2(orbPos, 0);

                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/Magic/magnetOrb_blue"), curOrbPos, new Rectangle(0, 0, 20, 20), lightColor, 0f, new Vector2(10, 10), 1, SpriteEffects.None, 0f);
            }
            else
            {
                curOrbPos = Projectile.Center - Main.screenPosition + new Vector2(-orbPos, 0);

                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/Magic/magnetOrb_orange"), curOrbPos, new Rectangle(0, 0, 20, 20), lightColor, 0f, new Vector2(10, 10), 1, SpriteEffects.None, 0f);
            }
            base.PostDraw(lightColor);
        }

    }
}