using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class Lightning_Spell:ModProjectile
    {

        Vector2 initPos;
        int[] lightningFrames=new int[150];
        int curFrame=1;
        int endFrame = 0;
        bool collided = false;
        int[] damagedPlayers;
        int curDamagedPlayer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning bolt");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 16;
            projectile.timeLeft = 150;
            projectile.penetrate = -1;
            projectile.rotation = (float)Math.PI/2;
            projectile.light = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            damagedPlayers = new int[Main.maxPlayers];
        }

        public override void AI()
        {
            endFrame += (collided) ? 1 : 0;
            if (projectile.timeLeft < 10)
            {
                return;
            }
            if (FindClosest() == -1 ||( endFrame >= curFrame && curFrame>0))
            {
                projectile.timeLeft = 1;
            }

            if (projectile.timeLeft > 148)
            {
                projectile.Center = new Vector2((projectile.friendly) ? Main.MouseWorld.X : projectile.Center.X, (projectile.friendly)?
                    Main.player[projectile.owner].Center.Y - 650:Main.player[FindClosest()].Center.Y-650);

                initPos = projectile.Center;
                projectile.velocity = new Vector2(0, 45);
            }

            projectile.ai[0] += (int)MathHelp.Magnitude(projectile.velocity*2);
            while (projectile.ai[0] >26)
            {
                lightningFrames[curFrame] = projectile.frame;
                projectile.frame = Main.rand.Next(0,4);
                curFrame++;
                projectile.ai[0] -= 26;
                
            }

            DamageEnemies();

            int sparks = Main.rand.Next(curFrame-endFrame);
            for(int i = 0; i < sparks*2; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 175);
            }

            for(int i = 0; i < 3; i++)
            {
                Vector2 pos = new Vector2(projectile.Center.X, Main.player[(projectile.friendly ? projectile.owner : FindClosest())].Center.Y - 20) ;
                Dust.NewDust(pos, projectile.width, projectile.height, 175);
            }

        }
        
        int FindClosest()
        {
            float distance = 10000;
            int closest=-1;
            for(int i = 0;i<Main.maxPlayers;i++)
            {
                if (Vector2.Distance(Main.player[i].Center, projectile.Center) < distance)
                {
                    distance = Vector2.Distance(Main.player[i].Center, projectile.Center);
                    closest = i;
                }
            }
            return closest;
        }

        void DamageEnemies()
        {
            if (projectile.friendly)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly)
                    {
                        if (MathHelp.IsInBounds(Main.npc[i].Center, initPos - new Vector2(projectile.width, 0), initPos + new Vector2(projectile.width, curFrame * 34)))
                        {
                            Main.PlaySound(Main.npc[i].HitSound, Main.npc[i].Center);
                            Main.npc[i].life -= projectile.damage;
                            Main.npc[i].velocity = new Vector2(MathHelp.Sign(Main.npc[i].Center.X - projectile.Center.X) * 5, -5);
                            Main.npc[i].checkDead();
                        }
                    }
                }
            }
            else if(projectile.hostile)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && !DamagedPlayer(i))
                    {
                        if (MathHelp.IsInBounds(Main.player[i].Center, initPos - new Vector2(projectile.width, 0), initPos + new Vector2(projectile.width, curFrame * 34)))
                        {
                            Main.PlaySound(SoundID.DD2_LightningAuraZap, Main.player[i].Center);
                            Main.player[i].Hurt(Terraria.DataStructures.PlayerDeathReason.ByProjectile(i,projectile.whoAmI), projectile.damage,0);

                            damagedPlayers[curDamagedPlayer] = i;
                            curDamagedPlayer++;
                        }
                    }
                }
            }
        }

        bool DamagedPlayer(int player)
        {
            for(int i = 0; i < damagedPlayers.Length; i++)
            {
                if (player == damagedPlayers[i])
                {
                    return true;
                }
            }
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);


            for (int i = endFrame; i < curFrame; i++)
            {
                Vector2 segPos = initPos + new Vector2(0,i * 26);
                Rectangle rect = new Rectangle(0, lightningFrames[curFrame] * 16, projectile.width, projectile.height);

                Texture2D texture = mod.GetTexture("Projectiles/Lightning_Spell");
                Vector2 origin = new Vector2((float)texture.Width / 2f, (float)texture.Height / 2f/4f);
                Color segColor = Lighting.GetColor((int)(segPos.X / 16f),(int)(segPos.Y/16f));
                segColor = projectile.GetAlpha(segColor);
                Lighting.AddLight(segPos / 16, new Vector3(1));

                spriteBatch.Draw(texture,segPos- Main.screenPosition, rect, segColor, projectile.rotation, origin, 1f, SpriteEffects.None, 0);
            }
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            collided = true;
            return false;
        }

    }
}
