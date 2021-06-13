using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class denseShard:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dense Shard");
            Tooltip.SetDefault("A gem fragment filled with constriction" +
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
