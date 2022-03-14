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
    public class stop:ModProjectile
    {

        public static string[] numbers = new string[] { "I","II","III","IV","V","VI","VII","VIII","IX","X","XI","XII" };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stop");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 100000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.alpha = 50;
        }

        public override bool PreAI()
        {
            if (Projectile.timeLeft > 60)
            {
                Projectile.timeLeft = 60;
            }
            if (Projectile.timeLeft > 50)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active && i != Projectile.whoAmI && Main.projectile[i].type == Projectile.type)
                    {
                        Projectile.timeLeft = 1;
                        Main.projectile[Projectile.whoAmI].active = false;
                        return false;
                    }
                }
            }
            return base.PreAI();
        }

        public override void AI()
        {
            Projectile.damage = 0;


            Player player =Main.player[Projectile.owner];
            if (Projectile.timeLeft >= 59)
            {
                foreach(NPC npc in Main.npc)
                {
                    if (npc.active && Vector2.Distance(npc.Center, Projectile.Center) < 500)
                    {
                        npc.AddBuff(ModContent.BuffType<Buffs.StoppedBuff>(),240);
                    }
                }
                foreach(Player p in Main.player)
                {
                    if(p.active && p!=Main.player[Projectile.owner] && Vector2.Distance(p.Center, Projectile.Center) < 500)
                    {
                        p.AddBuff(ModContent.BuffType<Buffs.StoppedBuff>(), 240);
                    }
                }
            }

            Projectile.scale =Math.Clamp( 4f*((60-Projectile.timeLeft)/60f*2),0,(Projectile.timeLeft<30)?Projectile.timeLeft/30*2:2);

            if (Projectile.timeLeft > 40 && player==Main.LocalPlayer)
            {
                player.velocity = Vector2.Zero;
                player.gravity = 0;
                CommandLogic.instance.ChangeCommand(0);
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/Magic/stop").Value;

            if (texture != null)
            {
                Rectangle rect = new Rectangle(0, texture.Height / 6, texture.Width, texture.Height / 6);
                Vector2 desPos = Projectile.Center + new Vector2(30 - (Projectile.timeLeft-50)*6-Projectile.width/2, -Projectile.height);

                if (Projectile.timeLeft > 50)
                {

                    Main.spriteBatch.Draw(texture, desPos - Main.screenPosition, rect, lightColor, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

                    desPos = Projectile.Center + new Vector2((Projectile.timeLeft-50)*6 - 30-Projectile.width/2, -Projectile.height);

                    Main.spriteBatch.Draw(texture, desPos - Main.screenPosition, rect, lightColor, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                }
                else
                {
                    desPos = Projectile.Center;
                    rect = new Rectangle(0, texture.Height / 6*2, texture.Width, texture.Height / 6);

                    Main.spriteBatch.Draw(texture, desPos - Main.screenPosition, rect, lightColor, 0, Vector2.Zero, Math.Clamp((Projectile.timeLeft-50),0,4), SpriteEffects.None, 0);

                    for(int i = 0; i < numbers.Length; i++)
                    {
                        desPos = Projectile.Center + new Vector2((float)Math.Sin(i/(float)numbers.Length*10f)*50f,0);
                        if (Projectile.timeLeft < 50 - i * 2)
                        {
                            DrawNumber(numbers[i], texture, desPos,(i>numbers.Length/2)?Color.White:Color.Gray, (float)Math.Sin(i / (float)numbers.Length)/2f+0.5f);
                        }
                    }

                }
            }

            return base.PreDraw(ref lightColor);
        }

        public void DrawNumber(string number,Texture2D texture,Vector2 desPos,Color clr,float scale=1)
        {
            Rectangle rect;
            int num;
            Vector2 finalPos;
            for (int i = 0; i < number.Length; i++)
            {
                finalPos = desPos;
                if (number.Length == 2)
                {
                    finalPos += new Vector2(20, 0)*(int)((i*2)-1);
                }

                switch (number[i])
                {
                    default:
                        return;
                    case 'I':
                        num = 3;
                        break;
                    case 'V':
                        num = 5;
                        break;
                    case 'X':
                        num = 4;
                        break;
                }
                rect = new Rectangle(0, texture.Height / 6 *num, texture.Width, texture.Height / 6);
                Main.spriteBatch.Draw(texture, finalPos - Main.screenPosition, rect, clr, 0, Vector2.Zero,scale, SpriteEffects.None, 0);
            }
        }

    }
}
