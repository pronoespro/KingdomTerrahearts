using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;

namespace KingdomTerrahearts.NPCs.Bosses
{
    [AutoloadBossHead]
    public class Darkside :ModNPC
    {

        Player player;
        public int bossAttackType = 0;
        public int shootAttackTimes = 2;
        public int shootAttacksUsed = 0;

        bool resizedInBattleGrounds;

        void Target()
        {
            player = Main.player[NPC.target];
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkside");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1500;
            NPC.damage = 0;
            NPC.defense = 15;
            NPC.knockBackResist = 0;
            NPC.width = 190/2;
            NPC.height = 200/2;
            NPC.alpha = 255;
            NPC.scale = 1.5f;
            NPC.value = 5000;
            NPC.npcSlots = 4;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.behindTiles = true;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/Vs Pure Heartless");
            NPC.ai[1] = 200;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.75f);
            NPC.defense = (int)(NPC.defense + numPlayers);
            NPC.scale = 1 + (0.5f * numPlayers);
        }

        public override void AI()
        {

            if (player != null)
            {
                SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                if (sp.fightingInBattleground || KingdomWorld.customInvasionUp)
                {
                    NPC.scale = (resizedInBattleGrounds) ? NPC.scale : NPC.scale * 1.5f;
                    if (sp.fightingInBattleground)
                        NPC.damage *= 2;
                    resizedInBattleGrounds = true;
                }
            }

            if (KingdomWorld.customInvasionUp) Music = -1;

            Target();

            NPC.ai[2]--;
            if (NPC.ai[2] <= 0)
            {
                Teleport();
            }

            DespawnHandler();

            if (NPC.alpha <= 2) {
                NPC.ai[1]--;
                if (NPC.ai[1] <= 0)
                {
                    if(bossAttackType==0)
                        bossAttackType = Main.rand.Next(1, 3);

                    bossAttack(bossAttackType);
                }
            }

        }

        void Teleport()
        {
            if (MathHelp.Magnitude(player.Center - NPC.Center) > NPC.width*4)
            {
                NPC.alpha += 5;
                if (NPC.alpha >= 255)
                {
                    NPC.alpha = 0;
                    if (player.active && !player.dead)
                        NPC.Center = player.Center + new Vector2(0, -150);
                    NPC.ai[2] = 50;
                }
            }
            else
            {
                NPC.alpha -= 5;
                if (NPC.alpha <= 0)
                    NPC.alpha = 0;
            }
        }

        void DespawnHandler()
        {
            if(!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0, 100000);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int frame;
            if (bossAttackType == 0)
            {
                NPC.frameCounter++;
                NPC.frameCounter %= 100;
                frame = (int)(NPC.frameCounter / 50);
            }
            else if(bossAttackType==1)
            {
                frame = 2;
            }
            else
            {
                if (NPC.ai[1] < -125)
                {
                    frame = 4;
                }
                else
                {
                    frame = 3;
                }
            }
            if (frame >= Main.npcFrameCount[NPC.type]) frame = 0;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.DarkenedHeart>(),1,2,2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lucidStone>(),1,5,15));
            npcLoot.Add(ItemDropRule.Common(ItemID.FallenStar,1, 15,15));
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            return null;
        }

        public void bossAttack(int attackType)
        {
            ProjectileSource_NPC s = new ProjectileSource_NPC(NPC);

            if (attackType == 1)
            {
                int projectile = ModContent.ProjectileType<Projectiles.DarksideMissileOrb>();
                if (NPC.ai[1] < -100)
                {
                    if (shootAttacksUsed < shootAttackTimes)
                    {
                        int missile = ModContent.NPCType<Projectiles.BossStuff.darksideMagicMissiles>();


                        NPC.NewNPC((int)NPC.Center.X + 15, (int)NPC.Center.Y, missile);
                        NPC.NewNPC((int)NPC.Center.X - 15, (int)NPC.Center.Y, missile);
                        //NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y + 15, missile);
                        //NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y - 15, missile);

                        shootAttacksUsed++;
                        NPC.ai[1]=0;
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
                        NPC.ai[1] = 200 + Main.rand.Next(200);
                        bossAttackType = 0;
                    }
                }
                else
                {
                    bool projectileExists = false;
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].type==projectile) {
                            Main.projectile[i].Center = NPC.Center;
                            Main.projectile[i].timeLeft = 15;
                            projectileExists = true;
                        }
                    }
                    if(!projectileExists)
                        Projectile.NewProjectile(s,NPC.Center, Vector2.Zero, projectile, 0, 0);
                }
            }
            else if(attackType==2)
            {
                if (NPC.ai[1] < -250)
                {
                    int missile = ModContent.NPCType<NPCs.shadowHeartless>();

                    NPC.NewNPC((int)NPC.Center.X + 15, (int)NPC.Center.Y, missile);
                    NPC.NewNPC((int)NPC.Center.X - 15, (int)NPC.Center.Y, missile);
                    NPC.ai[1] = 200 + Main.rand.Next(200);
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
            int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, 200, 2 * hitDirection, -2f);
            Main.dust[dust].color = Color.Black;
            Main.dust[dust].noGravity = true;
        }

        public override bool CheckDead()
        {
            KingdomWorld.downedDarkside = true;
            return base.CheckDead();
        }

    }
}
