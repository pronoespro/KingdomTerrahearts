using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class VergilSlice:ModProjectile
    {


        Vector2[] LineDirection;
        Vector2[] LineOffset;
        float[] lineTimeOffset;
        float[] lineGrowSpeed;
        float timeToLine;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 28;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 68;
            projectile.height = 56;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 55;
            projectile.scale = 1.1f;
            LineDirection = new Vector2[5];
            LineOffset = new Vector2[LineDirection.Length];
            lineTimeOffset = new float[LineDirection.Length];
            lineGrowSpeed = new float[LineDirection.Length];
            timeToLine = 35f / 2f;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.townNPC || !target.friendly;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int i = 0; i < LineDirection.Length; i++)
            {
                while (LineDirection[i] == Vector2.Zero)
                {
                    LineDirection[i] = new Vector2((Main.rand.NextFloat(1f) * 2f - 1f), (Main.rand.NextFloat(1f) * 2f - 1f));
                    LineOffset[i] = new Vector2(Main.rand.NextFloat(-75,75),Main.rand.NextFloat(-75,75));
                    LineDirection[i].Normalize();
                    lineTimeOffset[i] = Main.rand.NextFloat(timeToLine);
                    lineGrowSpeed[i] = Main.rand.Next(20, 45);
                }

                Texture2D texture = mod.GetTexture("Projectiles/ScepTend/VergilSlice_beam");
                float r = LineDirection[i].ToRotation() + (float)Math.PI / 2;

                Vector2 origin;

                float percentageDone = Math.Min(1, (((projectile.timeLeft-20f) / 35f) - (lineTimeOffset[i] / timeToLine)) * lineGrowSpeed[i]);
                Color c;
                for (int j =-500; j < 500-percentageDone * 1000f; j += 3)
                {
                    origin = projectile.Center+LineOffset[i] + LineDirection[i] * j;
                    c = new Color(250, 250, 250, 50);
                    spriteBatch.Draw(texture, origin - Main.screenPosition,
                        new Rectangle(0, 0, 10, 10), c, r,
                        new Vector2(10 / 2, 10 / 2), 0.3f, 0, 0);
                }
                if (percentageDone > 0.5f && percentageDone<1)
                {
                    texture = mod.GetTexture("Projectiles/ScepTend/vergilCloneAttack");
                    origin = projectile.Center + LineOffset[i] + LineDirection[i] * (500f - percentageDone * 1000f);
                    spriteBatch.Draw(texture, origin - Main.screenPosition,
                        new Rectangle(0, 0, 75, 51), Color.White, r,
                        new Vector2(75 / 2, 51 / 2), 1, 0, 0);
                }
            }
            
            return false;
        }

        public override void AI()
        {
            projectile.friendly = true;

            projectile.frame = (int)((1f-(projectile.timeLeft / 28f))*28f);

            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if (!Main.npc[i].friendly && !Main.npc[i].townNPC && Vector2.Distance(Main.npc[i].Center,projectile.Center)<500)
                { 
                    Main.npc[i].life -= projectile.damage;
                    Main.npc[i].checkDead();
                }
            }
            projectile.ai[0]++;
            if (projectile.ai[0] % 10 == 0)
            {
                int proj=Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("Vergil_Bubble"), 0, 0, projectile.owner);
                Main.projectile[proj].ai[0]= 50;
            }
        }

    }
}
