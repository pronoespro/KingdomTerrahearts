using KingdomTerrahearts.NPCs.Bosses;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.BossStuff
{
    public class darksideMagicMissiles:ModNPC
    {

        Player player;
        NPC target;
        float speed=5f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Missile");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.width = 15;
            npc.height = 25;
            npc.aiStyle = -1;
            npc.lifeMax = 2;
            npc.damage = 25;
            npc.defense = 0;
            npc.npcSlots = 0.1f;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.velocity = new Vector2(0, -5);
            npc.timeLeft = 1000;
        }

        public override void AI()
        {
            if(npc.collideX || npc.collideY)
            {
                npc.timeLeft = 0;
            }

            npc.immortal = false;
            npc.damage = 25;
            Target();
            Player p;
            if(!npc.friendly)
                p = Main.player[npc.target];

            if (DespawnHandler())
            {
                return;
            }
            Move(new Vector2(0, 0));
            npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + 1.57f;

        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            npc.frameCounter %= 10;
            int frame = (int)(npc.frameCounter / 2);
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
        }

        void Target()
        {
            if (!npc.friendly)
            {
                player = Main.player[npc.target];
            }
            else
            {
                int boss = NPC.FindFirstNPC(mod.NPCType("Darkside"));
                if (boss > 0)
                {
                    target = Main.npc[boss];
                }
                else
                    target = null;
            }
        }

        void Move(Vector2 offset)
        {
            float turnResistance = 50;
            Vector2 moveTo;
            if(npc.friendly)
                moveTo = (target.Center + offset) - npc.Center;
            else
                moveTo = (player.Center+offset)-npc.Center;
            float magnitude = Magnitude(moveTo);
            if (magnitude > speed)
            {
                moveTo *= speed / magnitude;
            }
            moveTo = (npc.velocity * turnResistance + moveTo) / (turnResistance + 1f);
            magnitude = Magnitude(moveTo);
            if (magnitude > speed)
            {
                moveTo *= speed / magnitude;
            }
            npc.velocity = moveTo;
        }

        float Magnitude(Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        bool DespawnHandler()
        {
            if (npc.friendly)
            {
                if (target == null)
                {
                    npc.timeLeft = 0;
                    npc.position += new Vector2(0, 10000);
                    return true;
                }
                else
                {
                    if (Vector2.Distance(target.Center, npc.Center) < 50)
                    {
                        target.life -= npc.damage;
                        if (target.life <= 0) target.life = 1;
                        npc.life = 0;
                        npc.timeLeft = 1;

                        for (int i = 0; i < 50; i++)
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 65, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
                            Main.dust[dust].color = Color.Black;
                            Main.dust[dust].noGravity = true;
                        }
                    }
                }
            }
            else
            {
                if (!player.active || player.dead)
                {
                    npc.TargetClosest(false);
                    player = Main.player[npc.target];
                    if (!player.active || player.dead)
                    {
                        npc.timeLeft = 1;
                        npc.position += new Vector2(0, 10000);

                        for (int i = 0; i < 50; i++)
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 65, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
                            Main.dust[dust].color = Color.Black;
                            Main.dust[dust].noGravity = true;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public override void DrawEffects(ref Color drawColor)
        {
            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 65, -npc.velocity.X/2, -npc.velocity.Y/2);
            Main.dust[dust].color = Color.Black;
            Main.dust[dust].noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            HitPlayerInAnyWay();
        }

        void HitPlayerInAnyWay()
        {
            if (npc.timeLeft > 10)
                npc.timeLeft = 1;

            for (int i = 0; i < 50; i++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 65, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
                Main.dust[dust].color = Color.Black;
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool CheckDead()
        {

            if (npc.timeLeft == 1)
            {
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 65, Main.rand.Next(-2, 2), Main.rand.Next(-2, 2));
                    Main.dust[dust].color = Color.Black;
                    Main.dust[dust].noGravity = true;
                }
                return true;
            }

            if (!npc.friendly)
            {
                npc.friendly = true;
                npc.velocity = new Vector2(0, -5);
                npc.lifeMax *= 5;
                npc.life = npc.lifeMax;
                
            }
            npc.life = npc.lifeMax / 2;
            return false;
        }

    }
}
