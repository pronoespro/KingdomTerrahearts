using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace KingdomTerrahearts.Items.Placeable
{
    class savePoint_Item:ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Save spot");
			Tooltip.SetDefault("A place where you can collect your thoughts");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 2000;
			item.createTile = TileType<Tiles.savepoint>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bed);
			recipe.AddIngredient(ItemID.FallenStar, 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
