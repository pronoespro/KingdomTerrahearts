using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.Localization;

namespace KingdomTerrahearts.NPCs.TownNPCs
{
    [AutoloadHead]
    class Neku : ModNPC
    {

        bool firstTalked;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Player");
            Main.npcFrameCount[NPC.type] = 26;
        }

        public override void SetDefaults()
        {
            AnimationType = NPCID.Guide;
            NPC.townNPC = true;
            NPC.CloneDefaults(NPCID.Guide);
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return NPC.downedBoss1|| NPC.downedBoss2||NPC.downedBoss3;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.PlayerInTown && !NPC.AnyNPCs(NPC.type)) ? 10 : 0;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
        }

        public override void AI()
        {

        }

        public override List<string> SetNPCNameList()/* tModPorter Suggestion: Return a list of names */
        {
            List<string> possibleNames = new List<string>();
            possibleNames.Add("Neku Sakuraba");
            return possibleNames;
        }

        public override string GetChat()
        {
            Random r = new Random();
            List<string> dialogOptions = new List<string>();

            if (!firstTalked)
            {
                dialogOptions.Add("''Help the world in need. You don't have a time limit.'' is the mission. What are you staring at? Go away!");
                firstTalked = true;
            }
            else
            {
                if (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3)
                {
                    dialogOptions.Add("Hey, nice job.");
                    dialogOptions.Add("I will help you if you need me.");
                    dialogOptions.Add("Do you like CAT?");
                }
                else
                {
                    dialogOptions.Add("Outta my face!!! You're blocking my view.");
                    dialogOptions.Add("Shut up!!! Stop talking.");
                    dialogOptions.Add("Just go the hell away!!!");
                    dialogOptions.Add("All the world needs is me. I've got my values... so you can keep yours, all right?");
                    dialogOptions.Add("I don't get people. Never have. Never will.");
                }
            }

            return dialogOptions[r.Next(0, dialogOptions.Count)];
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3)
                button = Language.GetTextValue("Shop");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3))
            {
                shop = true;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            if (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.quickRun>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType < Items.Heal>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType < Items.doubleJump>());
                nextSlot++;
                if (NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType < Items.glide>());
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType < Items.secondChance>());
                    nextSlot++;
                    if (Main.hardMode)
                    {
                        shop.item[nextSlot].SetDefaults(ModContent.ItemType < Items.MaxMobility>());
                        nextSlot++;

                        if (NPC.downedPlantBoss)
                        {
                            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.WielderSoul>());
                            nextSlot++;
                            if (NPC.downedMoonlord)
                            {
                                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Invincible>());
                                nextSlot++;
                            }
                        }
                    }
                }
            }
        }

    }
}
