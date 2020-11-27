using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    class lastWorldDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Last World Dye");
            Tooltip.SetDefault("Leave your body and become a specter");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(this, 3);
            recipe.AddRecipe();
        }

    }
}
