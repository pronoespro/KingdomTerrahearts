using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Placeable
{
    class twilightBlock:ModItem
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Town Block Fragment");
			Tooltip.SetDefault("A pice of a town far away" +
				"\nThe town is fine, this is just a lost fragment nobody cared about" +
	"\nThat nobody being Roxas");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("twilightTownBlock");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GrayBrick, 10);
			recipe.AddIngredient(mod.ItemType("twilightShard"));
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this, 99);
			recipe.AddRecipe();
		}

	}
}
