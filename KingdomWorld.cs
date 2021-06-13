using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using KingdomTerrahearts.NPCs.Invasions;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

namespace KingdomTerrahearts
{
    public class KingdomWorld : ModWorld
    {
        //Setting up variables for invasion
        public static bool customInvasionUp = false;
        public static bool downedCustomInvasion = false;

        //Setting up variables for bosses
        public static bool downedDarkside = false;
        public static bool[] downedXionPhases = new bool[] { false,false};

        //biome related
        public static int twilightBiome;

        //Initialize all variables to their default values
        public override void Initialize()
        {
            Main.invasionSize = 0;
            customInvasionUp = false;
            downedCustomInvasion = false;
        }

        //Save downed data
        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedCustomInvasion) downed.Add("thousandHeartless");
            if (downedDarkside) downed.Add("Darkside");

            for(int i = 0; i < downedXionPhases.Length; i++)
            {
                if (downedXionPhases[i])
                    downed.Add("XionPhase"+i.ToString());
            }

            return new TagCompound {
                {"downed", downed}
            };
        }

        //Load downed data
        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedCustomInvasion = downed.Contains("thousandHeartless");
            downedDarkside = downed.Contains("Darkside");

            for (int i = 0; i < downedXionPhases.Length; i++)
                downedXionPhases[i] = downed.Contains("XionPhase" + i.ToString());
        }

        //Sync downed data
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedCustomInvasion;
            writer.Write(flags);
        }

        //Sync downed data
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedCustomInvasion = flags[0];
        }

        //Allow to update invasion while game is running
        public override void PostUpdate()
        {
            if (customInvasionUp)
            {
                if (Main.invasionX == (double)Main.spawnTileX)
                {
                    //Checks progress and reports progress only if invasion at spawn
                    ThousandHeartlessInvasion.CheckCustomInvasionProgress();
                }
                //Updates the custom invasion while it heads to spawn point and ends it
                ThousandHeartlessInvasion.UpdateCustomInvasion();
            }
        }

        /*
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (genIndex == -1)
                return;
            tasks.Insert(genIndex + 1, new PassLegacy("Athens", delegate (GenerationProgress progress)
            {
                int x = WorldGen.genRand.Next(1, Main.maxTilesX - 300);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayer - 200, Main.maxTilesY - 200);
                int TileType = mod.TileType("twilightTownBlock");

                WorldGen.TileRunner(x, y, 350, WorldGen.genRand.Next(500, 700), TileType, false, 0, 0, true, true); 
            }));
        }*/

        public override void TileCountsAvailable(int[] tileCounts)
        {
            twilightBiome = tileCounts[mod.TileType("twilightTownBlock")];
        }

    }
}