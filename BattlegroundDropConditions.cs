using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace KingdomTerrahearts
{
    class BattlegroundDropConditions : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				// Can drop if it's not hardmode, and not a critter or an irrelevant enemy, and player is in the ExampleUndergroundBiome
				// Disclaimer: This is adapted from Conditions.SoulOfWhateverConditionCanDrop(info) to remove the cavern layer restriction, because ExampleUndergroundBiome also extends into the dirt layer

				NPC npc = info.npc;
				if (!npc.boss)
				{
					return false;
				}

				NPCOverride over =npc.GetGlobalNPC<NPCOverride>();

                if (over.heartlessVerActive)
                {
					return true;
                }
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return "Drops when fighting on the Battleground";
		}
	}
}
