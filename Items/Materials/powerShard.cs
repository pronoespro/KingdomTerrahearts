using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class powerShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril Shard");
            Tooltip.SetDefault("A mineral shard containing rare metal" +
                "\nUsed for item synthesis");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Blue;
            item.value = 200;
            item.maxStack = 999;
        }
    }
}
