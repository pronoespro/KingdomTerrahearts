using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Magic
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
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 16;
            Projectile.timeLeft = 150;
            Projectile.penetrate = -1;
            Projectile.rotation = (float)Math.PI/2;
            Projectile.light = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            damagedPlayers = new int[Main.maxPlayers];
        }

        public override void AI()
        {
            endFrame += (collided) ? 1 : 0;
            if (Projectile.timeLeft < 10)
            {
                return;
            }
            if (FindClosest() == -1 ||( endFrame >= curFrame && curFrame>0))
            {
                Projectile.timeLeft = 1;
            }

            if (Projectile.timeLeft > 148)
            {
                Projectile.Center = new Vector2((Projectile.friendly) ? Main.MouseWorld.X : Projectile.Center.X, (Projectile.friendly)?
                    Main.player[Projectile.owner].Center.Y - 650:Main.player[FindClosest()].Center.Y-650);

                initPos = Projectile.Center;
                Projectile.velocity = new Vector2(0, 45);
            }

            Projectile.ai[0] += (int)MathHelp.Magnitude(Projectile.velocity*2);
            while (Projectile.ai[0] >26)
            {
                lightningFrames[curFrame] = Projectile.frame;
                Projectile.frame = Main.rand.Next(0,4);
                curFrame++;
                Projectile.ai[0] -= 26;
                
            }

            DamageEnemies();

            int sparks = Main.rand.Next(curFrame-endFrame);
            for(int i = 0; i < sparks*2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 175);
            }

            for(int i = 0; i < 3; i++)
            {
                Vector2 pos = new Vector2(Projectile.Center.X, Main.player[(Projectile.friendly ? Projectile.owner : FindClosest())].Center.Y - 20) ;
                Dust.NewDust(pos, Projectile.width, Projectile.height, 175);
            }

        }
        
        int FindClosest()
        {
            float distance = 10000;
            int closest=-1;
            for(int i = 0;i<Main.maxPlayers;i++)
            {
                if (Vector2.Distance(Main.player[i].Center, Projectile.Center) < distance)
                {
                    distance = Vector2.Distance(Main.player[i].Center, Projectile.Center);
                    closest = i;
                }
            }
            return closest;
        }

        void DamageEnemies()
        {
            if (Projectile.friendly)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly)
                    {
                        if (MathHelp.IsInBounds(Main.npc[i].Center, initPos - new Vector2(Projectile.width, 0), initPos + new Vector2(Projectile.width, curFrame * 34)))
                        {
                            SoundEngine.PlaySound(Main.npc[i].HitSound.Value, Main.npc[i].Center);
                            Main.npc[i].life -= Projectile.damage;
                            Main.npc[i].velocity = new Vector2(MathHelp.Sign(Main.npc[i].Center.X - Projectile.Center.X) * 5, -5);
                            Main.npc[i].checkDead();
                        }
                    }
                }
            }
            else if(Projectile.hostile)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && !DamagedPlayer(i))
                    {
                        if (MathHelp.IsInBounds(Main.player[i].Center, initPos - new Vector2(Projectile.width, 0), initPos + new Vector2(Projectile.width, curFrame * 34)))
                        {
                            SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Main.player[i].Center);
                            Main.player[i].Hurt(Terraria.DataStructures.PlayerDeathReason.ByProjectile(i,Projectile.whoAmI), Projectile.damage,0);

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

        public override bool PreDraw(ref Color lightColor)
        {

            for (int i = endFrame; i < curFrame; i++)
            {
                Vector2 segPos = initPos + new Vector2(0, i * 26);
                Rectangle rect = new Rectangle(0, lightningFrames[curFrame] * 16, Projectile.width, Projectile.height);

                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/Magic/Lightning_Spell");
                Vector2 origin = new Vector2((float)texture.Width / 2f, (float)texture.Height / 2f / 4f);
                Color segColor = Lighting.GetColor((int)(segPos.X / 16f), (int)(segPos.Y / 16f));
                segColor = Projectile.GetAlpha(segColor);
                Lighting.AddLight(segPos / 16, new Vector3(1));

                Main.spriteBatch.Draw(texture, segPos - Main.screenPosition, rect, segColor, Projectile.rotation, origin, 1f, SpriteEffects.None, 0);
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
