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
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 100;
            projectile.scale = 1f;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 100000;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 300;
        }

        public override void AI()
        {

            orbRot++;

            orbPos = (float)(Math.Sin(orbRot/10f) * 40f);

            blueInFront = orbRot % 20 > 5 && orbRot % 20 < 15;

            projectile.frameCounter++;
            projectile.frame = (projectile.frameCounter / 7) % 3;

            if (projectile.damage > 0)
            {
                projectile.ai[1] = projectile.damage;
                projectile.damage = 0;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                magnetizedNpc = Main.npc[i];
                if (((magnetizedNpc.damage > 0 && !magnetizedNpc.friendly)) && magnetizedNpc.active)
                {
                    magnetizedNpc.velocity = (Vector2.Distance(magnetizedNpc.Center, projectile.Center) < 55) ? Vector2.Zero : (MathHelp.Normalize(projectile.Center - magnetizedNpc.Center) * 25);

                    magnetizedNpc.life -= (int)Math.Max(0f, ((projectile.ai[1]+1) / 10f + 1f)-Vector2.Distance(projectile.Center,magnetizedNpc.Center)/55f);

                    magnetizedNpc.checkDead();
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!blueInFront)
            {
                curOrbPos = projectile.Center - Main.screenPosition + new Vector2(orbPos, 0);

                spriteBatch.Draw(mod.GetTexture("Projectiles/Magic/magnetOrb_blue"), curOrbPos, new Rectangle(0, 0, 20, 20), lightColor, 0f, new Vector2(10, 10), 1, SpriteEffects.None, 0f);
            }
            else
            {
                curOrbPos = projectile.Center - Main.screenPosition + new Vector2(-orbPos, 0);

                spriteBatch.Draw(mod.GetTexture("Projectiles/Magic/magnetOrb_orange"), curOrbPos, new Rectangle(0, 0, 20, 20), lightColor, 0f, new Vector2(10, 10), 1, SpriteEffects.None, 0f);
            }

            return base.PreDraw(spriteBatch, lightColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (blueInFront)
            {
                curOrbPos = projectile.Center - Main.screenPosition + new Vector2(orbPos, 0);

                spriteBatch.Draw(mod.GetTexture("Projectiles/Magic/magnetOrb_blue"), curOrbPos, new Rectangle(0, 0, 20, 20), lightColor, 0f, new Vector2(10, 10), 1, SpriteEffects.None, 0f);
            }
            else
            {
                curOrbPos = projectile.Center - Main.screenPosition + new Vector2(-orbPos, 0);

                spriteBatch.Draw(mod.GetTexture("Projectiles/Magic/magnetOrb_orange"), curOrbPos, new Rectangle(0, 0, 20, 20), lightColor, 0f, new Vector2(10, 10), 1, SpriteEffects.None, 0f);
            }

            base.PostDraw(spriteBatch, lightColor);
        }

    }
}