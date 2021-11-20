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
                "\nA very common ingredient of Item synthesis");
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
