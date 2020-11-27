using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using KingdomTerrahearts.NPCs.Invasions;

namespace KingdomTerrahearts
{
    public class KingdomWorld : ModWorld
    {
        //Setting up variables for invasion
        public static bool customInvasionUp = false;
        public static bool downedCustomInvasion = false;

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

            return new TagCompound {
                {"downed", downed}
            };
        }

        //Load downed data
        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedCustomInvasion = downed.Contains("thousandHeartless");
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
    }
}