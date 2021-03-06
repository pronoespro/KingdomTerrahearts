﻿using Terraria;
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
    class donald : ModNPC
    {

        bool firstTalked;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Magician");
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
            return NPC.downedBoss1;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.playerInTown && !NPC.AnyNPCs(npc.type)) ? 10 : 0;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
        }

        public override void AI()
        {

        }

        public override string TownNPCName()
        {
            return "Donald";
        }

        public override string GetChat()
        {
            Random r = new Random();
            List<string> dialogOptions = new List<string>();

            if (!firstTalked)
            {
                dialogOptions.Add("Hello! I'm "+npc.FullName+"!");
                firstTalked = true;
            }
            else
            {
                if (NPC.AnyNPCs(mod.NPCType("goofy")))
                {
                    dialogOptions.Add("Goofy is the shield and I am the wand that will hit you in the head!");
                }
                else
                {
                    dialogOptions.Add("I hope Sora and Goofy are OK.");
                }
                dialogOptions.Add("WAHWAHWAHWAHWAHWAH!!!");
                dialogOptions.Add("Zettaflare...! Oh, sorry I was remembering something cool I did a while ago.");
                dialogOptions.Add("This might be a good spot to find some ingredients.");
            }

            return dialogOptions[r.Next(0, dialogOptions.Count)];
        }

    }
}
