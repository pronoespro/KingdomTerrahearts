using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomTerrahearts.NPCs.Invasions;
using KingdomTerrahearts.CustomTownNPCAI;
using System.Collections.Generic;
using KingdomTerrahearts.Extra;
using System;
using KingdomTerrahearts.Interface;
using Terraria.GameContent.ItemDropRules;

namespace KingdomTerrahearts.DropRules
{
    public class KingdomDropRules
    {

        public static IItemDropRule DropOnBattleground(SoraPlayer wielder,int itemID,int chanceDenominator=1,int minimumDropped=1,int maximumDropped=1)
        {
            if (wielder.fightingInBattlegrounds)
            {
                return ItemDropRule.Common(itemID,chanceDenominator,minimumDropped,maximumDropped);
            }
            else
            {
                return ItemDropRule.DropNothing();
            }
        }

        public static IItemDropRule DropOnBattlegroundWithRule(IItemDropRuleCondition condition,SoraPlayer wielder, int itemID, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
        {
            if (wielder.fightingInBattlegrounds)
            {
                return ItemDropRule.ByCondition(condition, itemID, chanceDenominator, minimumDropped, maximumDropped);
            }
            else
            {
                return ItemDropRule.DropNothing();
            }
        }

    }
}
