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

            customInvasionUp = true;
            downedCustomInvasion = false;

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
            flags[flagNum++] = customInvasionUp;
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
            customInvasionUp = flags[3];
        }

        //bool createdDonald;

        public override void PreUpdateWorld()
        {
            /*
            if (!createdDonald)
            {
                Mod crowdMod = ModLoader.GetMod("MobNPCs");
                if (crowdMod != null)
                {
                    crowdMod.Call("CreateCrowdSpace", ("NPCs/TownNPCs/donald"), 36, 1456 / 26, 0, 0, 500, 1, 1, 1f, 1f);
                }
                else
                {
                    Main.NewText("No crowd");
                }
                createdDonald = true;
            }
            */

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