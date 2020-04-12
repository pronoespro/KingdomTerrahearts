using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    class lucidShard:ModItem
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
            item.rare = 1;
            item.value = 200;
            item.maxStack = 999;
        }
        /*
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(this, 2);

            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult();
            recipe.AddRecipe();

        }
        */

    }
}
