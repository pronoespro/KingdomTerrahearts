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

    public class KHChestData
    {
        public Point position;
        public int type;
        public int stack;
        public bool opened;

        public KHChestData(int x, int y, int itemType, int ammount, bool open=false)
        {
            position = new Point(x, y);
            type = itemType;
            stack = ammount;
            opened = open;
        }

        public bool IsChest(int x,int y)
        {
            if (x >= position.X && x <= position.X + 1)
            {
                if (y <= position.Y && y >= position.Y - 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsChest(Point pos)
        {
            return IsChest(pos.X, pos.Y);
        }
    }

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
        public static int traverseBiome;

        //Chest related
        public static Dictionary<string,List<KHChestData>> treasureChests;
        public static string[] subworlds = new string[] { "Terraria" };
        public static string[] khBiomes= new string[] { "TwilightTown" };
        public static string khChestSave = "";

        public Dictionary<string,bool[]> backgroundStyleSwap;


        //Initialize all variables to their default values
        public static void Initialize()
        {

            customInvasionUp = false;
            downedCustomInvasion = false;

            treasureChests = new Dictionary<string, List<KHChestData>>();

            khChestSave = "";

        }

        public override void OnWorldLoad()
        {
            bool[] styleSwap;

            if (backgroundStyleSwap == null){
                backgroundStyleSwap = new Dictionary<string, bool[]>();
            }
            if (backgroundStyleSwap.Count == 0)
            {
                styleSwap = GetRandomBoolArray(2);
                backgroundStyleSwap.Add(khBiomes[0], styleSwap);
            }
            Backgrounds.TwilightTownBackground.SetupStyleSwap(backgroundStyleSwap[khBiomes[0]]);
        }

        public override void OnWorldUnload()
        {
            backgroundStyleSwap = null;
            Backgrounds.TwilightTownBackground.SetupStyleSwap(new bool[] { false, false });
        }

        public static bool[] GetRandomBoolArray(int length = 1)
        {
            bool[] randomArray = new bool[length];
            for (int i = 0; i < length; i++){
                randomArray[i] = Main.rand.NextBool();
            }
            return randomArray;
        }

        public static string GetWorldName()
        {
            return subworlds[0];
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

            if (khChestSave != null && khChestSave.Length > 0)
            {
                tag.Add("openChests", khChestSave);
                Mod.Logger.Debug("Hello "+khChestSave);
            }else{
                tag.Remove("openChests");
            }

            if (backgroundStyleSwap != null)
            {
                for (int i = 0; i < khBiomes.Length; i++)
                {
                    tag.Add(khBiomes[i],backgroundStyleSwap[khBiomes[i]]);
                }
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

            if (tag.ContainsKey("openChests"))
            {

                //chest
                treasureChests = new Dictionary<string, List<KHChestData>>();
                string chest = tag.GetString("openChests");
                if (chest != null)
                {
                    string[] d = chest.Split(';', StringSplitOptions.RemoveEmptyEntries);

                    if (d.Length == 0)
                    {
                        d = new string[] { chest };
                    }

                    Mod.Logger.Debug("Started loading chests");
                    string[] splitData;
                    foreach (string data in d)
                    {
                        splitData = data.Split('|');
                        if (splitData.Length > 1)
                        {
                            string[] splitPos = splitData[1].Split(',');

                            if (splitPos.Length > 1)
                            {
                                int x;
                                if (int.TryParse(splitPos[0], out x))
                                {
                                    Mod.Logger.Debug("X: " + x);
                                    int y;
                                    if (int.TryParse(splitPos[1], out y))
                                    {
                                        Mod.Logger.Debug("Y: " + y);
                                        int type;
                                        if (int.TryParse(splitPos[2], out type))
                                        {
                                            Mod.Logger.Debug("Type: " + type);
                                            int stack;
                                            if (int.TryParse(splitPos[3], out stack))
                                            {
                                                Mod.Logger.Debug("Stack: " + stack);
                                                if (!treasureChests.ContainsKey(splitData[0])){
                                                    treasureChests.Add(splitData[0], new List<KHChestData>());
                                                }
                                                treasureChests[splitData[0]].Add(new KHChestData(x, y, type, stack, splitPos.Length > 4));
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }

                }
            }
            
            for(int i = 0; i < khBiomes.Length; i++)
            {
                if (tag.ContainsKey(khBiomes[i]))
                {
                    Mod.Logger.Debug(khBiomes[i] + " was loaded");
                    if (backgroundStyleSwap.ContainsKey(khBiomes[i]))
                    {
                        backgroundStyleSwap[khBiomes[i]] = tag.Get<bool[]>(khBiomes[i]);
                    }
                    else
                    {
                        backgroundStyleSwap.Add(khBiomes[i], tag.Get<bool[]>(khBiomes[i]));
                    }
                }
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
            /*traverseBiome = tileCounts[ModContent.TileType<Tiles.TraverseTown_bricks>()]+
                tileCounts[ModContent.TileType<Tiles.TraverseTown_cobblestone>()]+
                tileCounts[ModContent.TileType<Tiles.TraverseTown_clay>()]+
                tileCounts[ModContent.TileType<Tiles.TraverseTown_Mail>()]*100;*/
        }

        public static void AddChest(int x, int y, int type, int stack)
        {
            if (treasureChests == null || !treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests = new Dictionary<string, List<KHChestData>>();
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }

            if (!treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }

            Point pos = new Point(x, y);

            for (int i = 0; i < treasureChests[GetWorldName()].Count; i++)
            {
                if (treasureChests[GetWorldName()][i].IsChest(pos))
                {
                    treasureChests[GetWorldName()][i].type = type;
                    treasureChests[GetWorldName()][i].stack = stack;
                    SaveChests();
                    return;
                }
            }

            treasureChests[GetWorldName()].Add(new KHChestData(x, y, type, stack));
            SaveChests();

        }

        public static void OpenChest(int x, int y)
        {
            if (treasureChests == null || !treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests = new Dictionary<string, List<KHChestData>>();
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }

            if (!treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }
            Point pos = new Point(x, y);
            for (int i = 0; i < treasureChests[GetWorldName()].Count; i++)
            {
                if (treasureChests[GetWorldName()][i].IsChest(pos))
                {
                    Main.NewText("opened chest!");
                    treasureChests[GetWorldName()][i].opened = true;
                    SaveChests();
                }
            }
        }

        public static KHChestData RemoveChest(int x, int y)
        {
            if (treasureChests == null || !treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests = new Dictionary<string, List<KHChestData>>();
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }

            if (!treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }
            Point pos = new Point(x, y);
            for (int i = 0; i < treasureChests[GetWorldName()].Count; i++)
            {
                if (treasureChests[GetWorldName()][i].IsChest(pos))
                {

                    Main.NewText("Destroyed chest at " + x.ToString() + "," + y.ToString());
                    treasureChests[GetWorldName()].RemoveAt(i);
                    SaveChests();
                }
            }
            return null;
        }

        public static bool HasChest(int x,int y)
        {
            if (treasureChests == null || !treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests = new Dictionary<string, List<KHChestData>>();
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }

            if (!treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }

            Point pos = new Point(x, y);
            for (int i = 0; i < treasureChests[GetWorldName()].Count; i++)
            {
                if (treasureChests[GetWorldName()][i].IsChest(pos))
                {
                    return true;
                }
            }

            return false;
        }

        public static KHChestData GetChest(int x, int y)
        {
            Point pos = new Point(x, y);
            for (int i = 0; i < treasureChests[GetWorldName()].Count; i++)
            {
                if (treasureChests[GetWorldName()][i].IsChest(pos))
                {
                    return treasureChests[GetWorldName()][i];
                }
            }
            return null;
        }

        public static void SaveChests()
        {
            KHChestData data;
            if (treasureChests.Keys.Count > 0)
            {
                string chests = "";

                foreach (string subworld in subworlds)
                {
                    if (KingdomWorld.treasureChests[subworld] != null && KingdomWorld.treasureChests[subworld].Count > 0)
                    {
                        for (int i = 0; i < KingdomWorld.treasureChests[subworld].Count; i++)
                        {
                            if (i != 0)
                            {
                                chests += ";";
                            }
                            data = KingdomWorld.treasureChests[subworld][i];
                            chests += subworld + "|" + data.position.X.ToString() + "," + data.position.Y.ToString()+","+ data.type.ToString()+ ","+ data.stack.ToString()+(data.opened?",true":"");
                        }
                    }
                }

                khChestSave = chests;

            }
            else
            {
                khChestSave = "";
            }
        }

        public static bool IsChestOpen(int x, int y)
        {
            if (treasureChests == null || !treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests = new Dictionary<string, List<KHChestData>>();
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }

            if (!treasureChests.ContainsKey(GetWorldName()))
            {
                treasureChests.Add(GetWorldName(), new List<KHChestData>());
            }



            for (int i = 0; i < treasureChests[GetWorldName()].Count; i++)
            {
                if (treasureChests[GetWorldName()][i] == null)
                {
                    treasureChests[GetWorldName()][i] = new KHChestData(x, y, ItemID.None, 0);
                }
                if (treasureChests[GetWorldName()][i].IsChest(new Point(x, y)))
                {
                    return treasureChests[GetWorldName()][i].opened;
                }
            }
            return false;
        }

    }
}