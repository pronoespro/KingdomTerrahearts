﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs.Invasions
{
    class ThousandHeartlessInvasion
    {

        public static int invasionSize;

        public static int[] heartless =
        {
            ModContent.NPCType<shadowHeartless>(),
            ModContent.NPCType<armoredKnightHeartless>(),
            ModContent.NPCType<SurveillanceRobotHeartless>()
        };

        public static int[] heartlessBosses ={};

        public static void StartInvasion()
        {

            heartlessBosses = new int[]{};
            heartless = new int[]{
                ModContent.NPCType<shadowHeartless>(),
                ModContent.NPCType<armoredKnightHeartless>(),
                ModContent.NPCType<SurveillanceRobotHeartless>()
            };

            Main.invasionType = 0;

            if (Main.invasionType == 0)
            {
                int numPlayers = 0;
                for(int i = 0; i < Main.maxPlayers; i++)
                {
                    numPlayers += (Main.player[i].active) ? 1 : 0;
                }
                if (numPlayers > 0)
                {
                    Main.invasionType = -1;
                    ///////To be continued
                    KingdomWorld.customInvasionUp = true;
                    invasionSize = (int)(1000 + (numPlayers) * 0.25f);
                    Main.invasionSize = invasionSize;
                    Main.invasionSizeStart = Main.invasionSize;
                    Main.invasionProgress = 0;
                    Main.invasionProgressIcon = 0 + 3;
                    Main.invasionProgressWave = 0;
                    Main.invasionProgressMax = Main.invasionSizeStart;
                    Main.invasionWarn = 3600; //This doesn't really matter, as this does not work, I like to keep it here anyways
                    Main.invasionX = 0; //Starts invasion immediately rather than wait for it to spawn
                }
            }

        }


        //Text for messages and syncing
        public static void CustomInvasionWarning()
        {
            String text = "";
            if (Main.invasionX == (double)Main.spawnTileX)
            {
                text = "The thousand heartless have reached the town!";
            }
            if (Main.invasionSize <= 0)
            {
                text = "The thousand heartless have been defeated.";
                KingdomWorld.downedCustomInvasion = true;
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, 175, 75, 255);
                return;
            }
            if (Main.netMode == NetmodeID.Server)
            {
                //Sync with net
                NetMessage.SendData(MessageID.ChatText, -1, -1, NetworkText.FromLiteral(text), 255, 175f, 75f, 255f, 0, 0, 0);
            }
        }

        public static void UpdateCustomInvasion()
        {
            //If the custom invasion is up
            if (KingdomWorld.customInvasionUp)
            {
                //End invasion if size less or equal to 0
                if (Main.invasionSize <= 0)
                {
                    KingdomWorld.customInvasionUp = false;
                    CustomInvasionWarning();
                    Main.invasionType = 0;
                    Main.invasionDelay = 0;
                    if (NPC.downedMoonlord)
                    {
                        EntitySource_OldOnesArmy s = new EntitySource_OldOnesArmy();
                        int npcSpawned=NPC.NewNPC(s,(int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, ModContent.NPCType<Bosses.heartlessXeanorth>());
                        Item.NewItem(s, Main.npc[npcSpawned].Center, ModContent.ItemType<Bosses.HeartlessXeanorthSpawner>(), noGrabDelay: true);
                    }
                }
                else if (Main.invasionSize <= invasionSize / 4 && NPC.downedPlantBoss)
                {
                    heartlessBosses = new int[] { ModContent.NPCType<Bosses.heartlessTide>(), ModContent.NPCType<Bosses.heartlessTower>() };
                    heartless = new int[] { ModContent.NPCType<shadowHeartless>() };
                }
                else if (Main.invasionSize <= invasionSize / 3 && Main.hardMode)
                {
                    heartlessBosses = new int[] { ModContent.NPCType<Bosses.heartlessTower>() };
                    heartless = new int[] { ModContent.NPCType<shadowHeartless>() };
                }
                else if (Main.invasionSize <= invasionSize / 4 * 3)
                {
                    heartlessBosses = new int[] { ModContent.NPCType<Bosses.Darkside>() };
                }

                //Do not do the rest if invasion already at spawn
                if (Main.invasionX == (double)Main.spawnTileX)
                {
                    return;
                }

                //Update when the invasion gets to Spawn
                float moveRate = (float)Main.dayRate * 15;

                //If the invasion is greater than the spawn position
                if (Main.invasionX > (double)Main.spawnTileX)
                {
                    //Decrement invasion x as to "move them"
                    Main.invasionX -= (double)moveRate;

                    //If less than the spawn pos, set invasion pos to spawn pos and warn players that invaders are at spawn
                    if (Main.invasionX <= (double)Main.spawnTileX)
                    {
                        Main.invasionX = (double)Main.spawnTileX;
                        CustomInvasionWarning();
                    }
                    else
                    {
                        Main.invasionWarn--;
                    }
                }
                else
                {
                    //Same thing as the if statement above, just it is from the other side
                    if (Main.invasionX < (double)Main.spawnTileX)
                    {
                        Main.invasionX += (double)moveRate;
                        if (Main.invasionX >= (double)Main.spawnTileX)
                        {
                            Main.invasionX = (double)Main.spawnTileX;
                            CustomInvasionWarning();
                        }
                        else
                        {
                            Main.invasionWarn--;
                        }
                    }
                }
            }
        }

        public static void CheckCustomInvasionProgress()
        {
            //Not really sure what this is
            if (Main.invasionProgressMode != 2)
            {
                Main.invasionProgressNearInvasion = false;
                return;
            }

            IAudioTrack track = MusicLoader.GetMusic("KingdomTerrahearts/Sounds/Music/SinesterShadows");
            if (!track.IsPlaying)
            {
                MusicLoader.GetMusic("KingdomTerrahearts/Sounds/Music/SinesterShadows").Play();
            }
            else
            {
                MusicLoader.GetMusic("KingdomTerrahearts/Sounds/Music/SinesterShadows").Update();
            }

            //Checks if NPCs are in the spawn area to set the flag, which I do not know what it does
            bool flag = false;
            Player player = Main.player[Main.myPlayer];
            Rectangle rectangle = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
            int num = 5000;
            int icon = 0;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active)
                {
                    icon = 0;
                    int type = Main.npc[i].type;
                    for (int n = 0; n < heartless.Length; n++)
                    {
                        if (type == heartless[n])
                        {
                            Rectangle value = new Rectangle((int)(Main.npc[i].position.X + (float)(Main.npc[i].width / 2f)) - num, (int)(Main.npc[i].position.Y + (float)(Main.npc[i].height / 2f)) - num, num * 2, num * 2);

                            if (rectangle.Intersects(value))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
            }
            Main.invasionProgressNearInvasion = flag;
            int progressMax3 = 1;

            //If the custom invasion is up, set the max progress as the initial invasion size
            if (KingdomWorld.customInvasionUp)
            {
                progressMax3 = Main.invasionSizeStart;
            }

            //If the custom invasion is up and the enemies are at the spawn pos
            if (KingdomWorld.customInvasionUp && (Main.invasionX == (double)Main.spawnTileX))
            {
                //Shows the UI for the invasion
                Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, progressMax3, icon, 0);
            }

            //Syncing start of invasion
            foreach (Player p in Main.player)
            {
                NetMessage.SendData(MessageID.InvasionProgressReport, p.whoAmI, -1, null, Main.invasionSizeStart - Main.invasionSize, (float)Main.invasionSizeStart, (float)(Main.invasionType + 3), 0f, 0, 0, 0);
            }
        }
    }
}
