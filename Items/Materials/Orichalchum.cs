using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class Orichalchum:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orichalchum");
            Tooltip.SetDefault("A rare and most valuable ore" +
                "\nUsed for item synthesis");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Quest;
            item.value = 2000000;
            item.maxStack = 999;
        }
    }
}
