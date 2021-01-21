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
    class Neku:ModNPC
    {

        bool firstTalked;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Neku Sakuraba");
            Main.npcFrameCount[npc.type] = 26;
        }

        public override void SetDefaults()
        {
            animationType = NPCID.Guide;
            npc.townNPC = true;
            npc.CloneDefaults(NPCID.Guide);
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.playerInTown && !NPC.AnyNPCs(npc.type))?10:0;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection= npc.direction;
        }

        public override void AI()
        {

        }

        public override string TownNPCName()
        {
            return "Neku";
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

            return dialogOptions[r.Next(0,dialogOptions.Count)];
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            if(NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemID.DeathSickle);
                nextSlot++;
            }
        }

    }
}
