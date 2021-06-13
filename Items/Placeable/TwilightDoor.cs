using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Placeable
{
    class TwilightDoor:ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Town Door Fragment");
			Tooltip.SetDefault("A pice of a town far away" +
				"\nThe town is fine, this is just a lost fragment nobody cared about" +
	"\nThat nobody being Roxas");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 28;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 150;
			item.createTile = mod.TileType("TwilightDoorClosed");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("twilightBlock"), 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
