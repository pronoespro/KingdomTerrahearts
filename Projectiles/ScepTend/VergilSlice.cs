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
    public class VergilSlice:ModProjectile
    {


        Vector2[] LineDirection;
        Vector2[] LineOffset;
        float[] lineTimeOffset;
        float[] lineGrowSpeed;
        float timeToLine;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 28;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 68;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 55;
            Projectile.scale = 1.1f;
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

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < LineDirection.Length; i++)
            {
                while (LineDirection[i] == Vector2.Zero)
                {
                    LineDirection[i] = new Vector2((Main.rand.NextFloat(1f) * 2f - 1f), (Main.rand.NextFloat(1f) * 2f - 1f));
                    LineOffset[i] = new Vector2(Main.rand.NextFloat(-75, 75), Main.rand.NextFloat(-75, 75));
                    LineDirection[i].Normalize();
                    lineTimeOffset[i] = Main.rand.NextFloat(timeToLine);
                    lineGrowSpeed[i] = Main.rand.Next(20, 45);
                }

                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/ScepTend/VergilSlice_beam");
                float r = LineDirection[i].ToRotation() + (float)Math.PI / 2;

                Vector2 origin;

                float percentageDone = Math.Min(1, (((Projectile.timeLeft - 20f) / 35f) - (lineTimeOffset[i] / timeToLine)) * lineGrowSpeed[i]);
                Color c;
                for (int j = -500; j < 500 - percentageDone * 1000f; j += 3)
                {
                    float curPercent =(j+500f)/1000f;
                    origin = Projectile.Center + LineOffset[i] + LineDirection[i] * j;
                    c = new Color(250*curPercent, 250*curPercent, 250*curPercent, 50);
                    Main.spriteBatch.Draw(texture, origin - Main.screenPosition,
                        new Rectangle(0, 0, 10, 10), c, r,
                        new Vector2(10 / 2, 10 / 2), 0.3f, 0, 0);
                }
                if (percentageDone > 0.5f && percentageDone < 1)
                {
                    texture = (Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/ScepTend/vergilCloneAttack");
                    origin = Projectile.Center + LineOffset[i] + LineDirection[i] * (500f - percentageDone * 1000f);
                    Main.spriteBatch.Draw(texture, origin - Main.screenPosition,
                        new Rectangle(0, 0, 75, 51), Color.White, r,
                        new Vector2(75 / 2, 51 / 2), 1, 0, 0);
                }
            }

            return false;
        }

        public override void AI()
        {
            int vergilProjCount = 0;
            for(int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type==Type)
                {
                    vergilProjCount++;
                }
            }
            if (vergilProjCount > 7)
            {
                Projectile.timeLeft = 0;
                return;
            }

            Projectile.friendly = true;

            Projectile.frame = (int)((1f-(Projectile.timeLeft / 28f))*28f);

            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if (!Main.npc[i].friendly && !Main.npc[i].townNPC && Vector2.Distance(Main.npc[i].Center,Projectile.Center)<500)
                { 
                    Main.npc[i].life -= Projectile.damage;
                    Main.npc[i].checkDead();
                }
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 10 == 0)
            {
                ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);

                int proj=Projectile.NewProjectile(s,Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Vergil_Bubble>(), 0, 0, Projectile.owner);
                Main.projectile[proj].ai[0]= 50;
            }
        }

    }
}
