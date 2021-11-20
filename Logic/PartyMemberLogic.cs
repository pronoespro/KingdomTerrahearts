using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using KingdomTerrahearts.Extra;
using Terraria.DataStructures;
using System;

namespace KingdomTerrahearts.Logic
{
    public class PartyMemberLogic
    {

        public static Dictionary<string, List<int>> partyMembers;

        public static void Reset()
        {
            partyMembers = new Dictionary<string, List<int>>();
        }

        public static int IsPartyMember(int npcType)
        {
            foreach(string playerName in partyMembers.Keys)
            {
                for(int i = 0; i < partyMembers[playerName].Count; i++)
                {
                    if (npcType == partyMembers[playerName][i])
                    {
                        for(int p = 0; p < Main.maxPlayers; p++)
                        {
                            if(Main.player[p].active && !Main.player[p].dead)
                            {
                                return p;
                            }
                        }
                    }
                }
            }
            return -1;
        }

        public static void AddPartyMember(int type,string player="")
        {
            List<int> partyMemb = new List<int>();

            string playerName = (player == "") ? Main.LocalPlayer.name : player;

            if (!partyMembers.ContainsKey(playerName))
            {
                partyMemb.Add(type);
                partyMembers.Add(playerName, partyMemb);
            }
            else
            {
                if (partyMembers[player].Count<3)
                {
                    partyMembers.TryGetValue(playerName, out partyMemb);
                    partyMembers.Remove(playerName);
                    partyMemb.Add(type);
                    partyMembers.Add(playerName, partyMemb);
                }
            }
        }

        public static void RemovePartyMember(int type)
        {
            foreach(string key in partyMembers.Keys)
            {
                for(int i = 0; i < partyMembers[key].Count; i++)
                {
                    if (partyMembers[key][i] == type)
                    {
                        partyMembers[key].RemoveAt(i);
                    }
                }
            }
        }

        public static void GetNetPartymembers(string player, List<int> npcs)
        {
            if (partyMembers.ContainsKey(player))
            {
                partyMembers.Remove(player);
            }
            partyMembers.Add(player, npcs);
        }

        public static int GetPartySlotsLeft(string playerName)
        {
            if (partyMembers.ContainsKey(playerName))
            {
                return 3 - partyMembers[playerName].Count;
            }
            return 3;
        }

        public static int GetPartySlotOcupied(int partyMemberType)
        {
            foreach(string key in partyMembers.Keys)
            {
                for(int i = 0; i < partyMembers[key].Count; i++)
                {
                    if (partyMembers[key][i] == partyMemberType)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

    }
}
