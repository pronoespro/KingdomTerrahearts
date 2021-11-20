using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace KingdomTerrahearts.Items
{
    public class Betsys_HatchingEgg:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Betsy's Hatching Egg");
            Tooltip.SetDefault("Spawn Betsy");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.useStyle=ItemUseStyleID.HoldUp;
            Item.useTime = Item.useAnimation = 30;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 1000;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(NPCID.DD2Betsy) || base.CanUseItem(player);
        }

        public override bool? UseItem(Player player)
        {

            NPC.SpawnBoss((int)player.Center.X, (int)Main.screenPosition.Y, NPCID.DD2Betsy, player.whoAmI);

            return base.UseItem(player);
        }

    }
}
