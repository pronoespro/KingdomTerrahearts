using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class lucidShard:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lucid Shard");
            Tooltip.SetDefault("A gem shard containing essence of emptiness" +
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
