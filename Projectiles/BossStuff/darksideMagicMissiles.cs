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
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 15;
            NPC.height = 25;
            NPC.aiStyle = -1;
            NPC.lifeMax = 2;
            NPC.damage = 25;
            NPC.defense = 0;
            NPC.npcSlots = 0.1f;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.velocity = new Vector2(0, -5);
            NPC.timeLeft = 1000;
        }

        public override void AI()
        {
            if(NPC.collideX || NPC.collideY)
            {
                NPC.timeLeft = 0;
            }

            NPC.immortal = false;
            NPC.damage = 25;
            Target();
            Player p;
            if(!NPC.friendly)
                p = Main.player[NPC.target];

            if (DespawnHandler())
            {
                return;
            }
            Move(new Vector2(0, 0));
            NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;

        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            NPC.frameCounter %= 10;
            int frame = (int)(NPC.frameCounter / 2);
            if (frame >= Main.npcFrameCount[NPC.type]) frame = 0;
            NPC.frame.Y = frame * frameHeight;
        }

        void Target()
        {
            if (!NPC.friendly)
            {
                player = Main.player[NPC.target];
            }
            else
            {
                int boss = NPC.FindFirstNPC(ModContent.NPCType<NPCs.Bosses.Darkside>());
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
            if(NPC.friendly)
                moveTo = (target.Center + offset) - NPC.Center;
            else
                moveTo = (player.Center+offset)-NPC.Center;
            float magnitude = Magnitude(moveTo);
            if (magnitude > speed)
            {
                moveTo *= speed / magnitude;
            }
            moveTo = (NPC.velocity * turnResistance + moveTo) / (turnResistance + 1f);
            magnitude = Magnitude(moveTo);
            if (magnitude > speed)
            {
                moveTo *= speed / magnitude;
            }
            NPC.velocity = moveTo;
        }

        float Magnitude(Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        bool DespawnHandler()
        {
            if (NPC.friendly)
            {
                if (target == null)
                {
                    NPC.timeLeft = 0;
                    NPC.position += new Vector2(0, 10000);
                    return true;
                }
                else
                {
                    if (Vector2.Distance(target.Center, NPC.Center) < 50)
                    {
                        target.life -= NPC.damage;
                        if (target.life <= 0) target.life = 1;
                        NPC.life = 0;
                        NPC.timeLeft = 1;

                        for (int i = 0; i < 50; i++)
                        {
                            int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 65, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
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
                    NPC.TargetClosest(false);
                    player = Main.player[NPC.target];
                    if (!player.active || player.dead)
                    {
                        NPC.timeLeft = 1;
                        NPC.position += new Vector2(0, 10000);

                        for (int i = 0; i < 50; i++)
                        {
                            int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 65, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
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
            int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 65, -NPC.velocity.X/2, -NPC.velocity.Y/2);
            Main.dust[dust].color = Color.Black;
            Main.dust[dust].noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            HitPlayerInAnyWay();
        }

        void HitPlayerInAnyWay()
        {
            if (NPC.timeLeft > 10)
                NPC.timeLeft = 1;

            for (int i = 0; i < 50; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 65, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
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

            if (NPC.timeLeft == 1)
            {
                for (int i = 0; i < 50; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 65, Main.rand.Next(-2, 2), Main.rand.Next(-2, 2));
                    Main.dust[dust].color = Color.Black;
                    Main.dust[dust].noGravity = true;
                }
                return true;
            }

            if (!NPC.friendly)
            {
                NPC.friendly = true;
                NPC.velocity = new Vector2(0, -5);
                NPC.lifeMax *= 5;
                NPC.life = NPC.lifeMax;
                
            }
            NPC.life = NPC.lifeMax / 2;
            return false;
        }

    }
}
