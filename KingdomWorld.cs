using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using KingdomTerrahearts.NPCs.Invasions;
using Terraria.GameContent.Generation;
using Microsoft.Xna.Framework;
using KingdomTerrahearts.Extra;
using Terraria.UI;

namespace KingdomTerrahearts
{

    public class KingdomWorld : ModSystem
    {
        //Setting up variables for invasion
        public static bool customInvasionUp = false;
        public static bool downedCustomInvasion = false;

        //Setting up variables for bosses
        public static bool downedDarkside = false;
        public static bool[] downedXionPhases = new bool[] { false,false,false};

        //biome related
        public static int twilightBiome;


        //Initialize all variables to their default values
        public void Initialize()
        {

            customInvasionUp = false;
            downedCustomInvasion = false;

        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (customInvasionUp && Main.invasionSize>0 && Main.invasionSizeStart>0)
            {
                tag.Add("invasionUp",true);
                tag.Add("invationSize", Main.invasionSize);
                tag.Add("invationInitSize", Main.invasionSizeStart);
            }
            else
            {
                tag.Remove("invasionUp");
                tag.Remove("invationSize");
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("invasionUp"))
            {
                customInvasionUp = true;
                Main.invasionSize = tag.GetAsInt("invationSize");
                Main.invasionSizeStart = tag.GetAsInt("invationInitSize");
                ThousandHeartlessInvasion.invasionSize = Main.invasionSizeStart;
            }
        }

        //Sync downed data
        public override void NetSend(BinaryWriter writer)
        {
            int flagNum = 0;

            BitsByte flags = new BitsByte();
            
            flags[flagNum++] = downedCustomInvasion;
            flags[flagNum++] = downedDarkside;
            for(int i = 0; i < downedXionPhases.Length; i++)
            {
                flags[flagNum++] = downedXionPhases[i];
            }
            //flags[flagNum++] = customInvasionUp;
            writer.Write(flags);
        }

        //Sync downed data
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedCustomInvasion = flags[0];
            downedDarkside = flags[1];
            for (int i = 0; i < downedXionPhases.Length; i++) {
                downedXionPhases[i] =flags[2+i];
            }
            //customInvasionUp = flags[3];
        }

        //bool createdDonald;

        public override void PreUpdateWorld()
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


        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            twilightBiome = tileCounts[ModContent.TileType<Tiles.twilightTownBlock>()];
        }

    }
}