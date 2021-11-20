using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class frostShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Shard");
            Tooltip.SetDefault("A gem shard containing essence of ice" +
                "\nUsed for Item synthesis");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }
    }
}
