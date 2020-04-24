using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Placeable
{
    class TwilightWorkbench:ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twiligth Night Table");
			Tooltip.SetDefault("Night Table that you  can use for crafting as well.");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 14;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 150;
			item.createTile=mod.TileType("TwilightWorkbench");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("twilightBlock"), 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}
