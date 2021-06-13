using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class twilightShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight Shard");
            Tooltip.SetDefault("A gem fragment filled with twilight" +
                "\nA very common ingredient of item synthesis");
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