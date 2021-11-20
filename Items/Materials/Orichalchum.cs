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
                "\nUsed for Item synthesis");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Quest;
            Item.value = 2000000;
            Item.maxStack = 999;
        }
    }
}
