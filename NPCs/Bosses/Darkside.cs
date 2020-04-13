using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs.Bosses
{
    [AutoloadBossHead]
    class Darkside:ModNPC
    {

        Player player;
        public float speed;
        public int bossAttackType = 0;
        public int shootAttackTimes = 2;
        public int shootAttacksUsed = 0;

        void Target()
        {
            player = Main.player[npc.target];
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkside");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 2500;
            npc.damage = 0;
            npc.defense = 15;
            npc.knockBackResist = 0;
            npc.width = 190/2;
            npc.height = 200/2;
            npc.alpha = 255;
            npc.scale = 1.5f;
            npc.value = 5000;
            npc.npcSlots = 4;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.behindTiles = true;
            music = MusicID.Boss1;
            npc.ai[1] = 200;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.75f);
            npc.defense = (int)(npc.defense + numPlayers);
        }

        public override void AI()
        {
            Target();

            npc.ai[2]--;
            if (npc.ai[2] <= 0)
            {
                Teleport();
            }

            DespawnHandler();

            if (npc.alpha <= 2) {
                npc.ai[1]--;
                if (npc.ai[1] <= 0)
                {
                    if(bossAttackType==0)
                        bossAttackType = Main.rand.Next(1, 3);

                    bossAttack(bossAttackType);
                }
            }

        }

        void Teleport()
        {
            if (Magnitude(player.Center - npc.Center) > npc.width*4)
            {
                npc.alpha += 5;
                if (npc.alpha >= 255)
                {
                    npc.alpha = 0;
                    if (player.active && !player.dead)
                        npc.Center = player.Center + new Vector2(0, -150);
                    npc.ai[2] = 50;
                }
            }
            else
            {
                npc.alpha -= 5;
                if (npc.alpha <= 0)
                    npc.alpha = 0;
            }
        }
        float Magnitude(Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        void DespawnHandler()
        {
            if(!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0, 100000);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int frame;
            if (bossAttackType == 0)
            {
                npc.frameCounter++;
                npc.frameCounter %= 100;
                frame = (int)(npc.frameCounter / 50);
            }
            else if(bossAttackType==1)
            {
                frame = 2;
            }
            else
            {
                if (npc.ai[1] < -125)
                {
                    frame = 4;
                }
                else
                {
                    frame = 3;
                }
            }
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), mod.ItemType("DarkenedHeart"),Stack:2);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            return null;
        }

        public void bossAttack(int attackType)
        {
            if (attackType == 1)
            {
                int projectile = mod.ProjectileType("DarksideMissileOrb");
                if (npc.ai[1] < -100)
                {
                    if (shootAttacksUsed < shootAttackTimes)
                    {
                        int missile = mod.NPCType("darksideMagicMissiles");


                        NPC.NewNPC((int)npc.Center.X + 15, (int)npc.Center.Y, missile);
                        NPC.NewNPC((int)npc.Center.X - 15, (int)npc.Center.Y, missile);
                        //NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 15, missile);
                        //NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 15, missile);

                        shootAttacksUsed++;
                        npc.ai[1]=0;
                    }
                    else
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            if (Main.projectile[i].type == projectile)
                            {
                                Main.projectile[i].timeLeft = 1;
                            }
                        }
                        shootAttacksUsed = 0;
                        npc.ai[1] = 200 + Main.rand.Next(200);
                        bossAttackType = 0;
                    }
                }
                else
                {
                    bool projectileExists = false;
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].type==projectile) {
                            Main.projectile[i].Center = npc.Center;
                            Main.projectile[i].timeLeft = 15;
                            projectileExists = true;
                        }
                    }
                    if(!projectileExists)
                        Projectile.NewProjectile(npc.Center, Vector2.Zero, projectile, 0, 0);
                }
            }
            else if(attackType==2)
            {
                if (npc.ai[1] < -250)
                {
                    int missile = mod.NPCType("shadowHeartless");

                    NPC.NewNPC((int)npc.Center.X + 15, (int)npc.Center.Y, missile);
                    NPC.NewNPC((int)npc.Center.X - 15, (int)npc.Center.Y, missile);
                    npc.ai[1] = 200 + Main.rand.Next(200);
                    bossAttackType = 0;
                }
            }
        }

        public override void DrawEffects(ref Color drawColor)
        {
            base.DrawEffects(ref drawColor);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int dust = Dust.NewDust(npc.Center, npc.width, npc.height, 200, 2 * hitDirection, -2f);
            Main.dust[dust].color = Color.Black;
            Main.dust[dust].noGravity = true;
        }

    }
}
