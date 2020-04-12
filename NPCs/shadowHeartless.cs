using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs
{
    class shadowHeartless : ModNPC
    {

        float desiredVel=0;
        float maxVel = 2;
        float teleportTime=240;
        float jumpForce = 8;
        bool followPlayers=true;
        int curFrame = 0;
        int walkcycle = 30;

        Vector2 lastPos=new Vector2();
        float notMovedTime=0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow heartless");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 0;
            npc.lifeMax = 50;
            npc.damage = 20;
            npc.defense = 1;
            npc.knockBackResist = 1;
            npc.width = 38;
            npc.height = 30;
            npc.value = 100;
            npc.npcSlots = 0.1f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;
        }

        public override void AI()
        {

            npc.TargetClosest(true);
            Player p = Main.player[npc.target];

            teleportTime--;

            if (teleportTime > 0)
            {
                npc.immortal = false;

                Vector2 moveTo = p.Center - npc.Center;

                if (npc.wet)
                {
                    npc.velocity.Y = -jumpForce;
                }

                if (followPlayers)
                {

                    desiredVel+= Math.Sign(moveTo.X) * 0.5f;
                    desiredVel = (Math.Abs(desiredVel) > maxVel/5) ? Math.Sign(desiredVel) * maxVel/5 : desiredVel;
                    
                    npc.velocity.X += desiredVel;
                    if (Math.Abs(npc.velocity.X) > maxVel) npc.velocity.X = Math.Sign(npc.velocity.X) * maxVel;
                    
                    if (lastPos == npc.Center)
                    {
                        npc.velocity.Y = -jumpForce;
                    }
                    if (lastPos.X == npc.Center.X)
                    {
                        notMovedTime++;
                        if (notMovedTime > 120)
                        {
                            followPlayers = false;
                            notMovedTime = 0;
                        }
                    }


                }
                else
                {

                    npc.velocity.X += -moveTo.X / Math.Abs(moveTo.X) * .25f;
                    npc.velocity.X = (Math.Abs(npc.velocity.X) > 2) ? npc.velocity.X / Math.Abs(npc.velocity.X) * 2 : npc.velocity.X;

                    if (lastPos == npc.Center)
                    {
                        npc.velocity.Y = jumpForce;
                    }

                    notMovedTime++;
                    if (notMovedTime > 120)
                    {
                        followPlayers = true;
                        notMovedTime = 0;
                    }


                }

                curFrame = (npc.velocity.Y != 0) ? 3 : (curFrame<3)?curFrame:0;

                lastPos = npc.Center;

            }
            else if (teleportTime>-20)
            {
                npc.frameCounter = 1;
                npc.immortal = true;
            }
            else
            {
                teleportTime = 240;
            }
            //npc.direction = (npc.velocity.X < 0) ? 1 : -1;

        }

        public override void FindFrame(int frameHeight)
        {
            if (curFrame < 3)
            {
                walkcycle--;
                walkcycle = (walkcycle < -30) ? 30 : walkcycle;
                int walk = 3;
                if (walkcycle > -15) walk = 2;
                if (walkcycle > 0) walk = 1;
                if (walkcycle > 15) walk = 0;

                switch (walk)
                {
                    case 0:
                    case 2:
                        curFrame = 0;
                        break;
                    case 1:
                        curFrame = 1;
                        break;
                    case 3:
                    default:
                        curFrame = 2;
                        break;
                }
            }

            npc.frame.Y = frameHeight * curFrame;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return  SpawnCondition.OverworldDaySlime.Chance/2;
        }

        public override void NPCLoot()
        {
            Random r = new Random();
            if (r.Next(100) < 5)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("lucidShard"), Main.rand.Next(5));
            }

            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CorruptSeeds, Main.rand.Next(1));
        }

    }
}
