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
    class goofy : ModNPC
    {

        bool firstTalked;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Captain of the Royal Knights");
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
            return NPC.downedBoss1;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.playerInTown && !NPC.AnyNPCs(NPC.type)) ? 10 : 0;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
        }

        public override void AI()
        {

        }

        public override string TownNPCName()
        {
            return "Goofy";
        }

        public override string GetChat()
        {
            Random r = new Random();
            List<string> dialogOptions = new List<string>();

            if (!firstTalked)
            {
                dialogOptions.Add("I'm " + NPC.FullName + ", but you can call me Goofy.");
                firstTalked = true;
            }
            else
            {
                if (NPC.AnyNPCs(ModContent.NPCType<donald>()))
                {
                    dialogOptions.Add("Donald might overdo it a little, but he is my friend.");
                }
                else
                {
                    dialogOptions.Add("I hope Sora and Donald are OK.");
                }
                dialogOptions.Add("My shield's ready!");
                dialogOptions.Add("Shield sliding is really fun. You should try it some time!");
            }

            return dialogOptions[r.Next(0, dialogOptions.Count)];
        }

    }
}
