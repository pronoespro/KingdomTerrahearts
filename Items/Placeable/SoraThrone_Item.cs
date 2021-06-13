using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Placeable
{
    class SoraThrone_Item:ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sora Throne");
			Tooltip.SetDefault("A throne fit for a king..." +
				"\nor Sora I guess");
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
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 150;
			item.createTile = mod.TileType("SoraThrone");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Throne);
			recipe.AddIngredient(mod.ItemType("blazingShard"),100);
			recipe.AddIngredient(mod.ItemType("denseShard"),100);
			recipe.AddIngredient(mod.ItemType("frostShard"),100);
			recipe.AddIngredient(mod.ItemType("lucidShard"),100);
			recipe.AddIngredient(mod.ItemType("powerShard"), 100);
			recipe.AddIngredient(mod.ItemType("pulsingShard"),100);
			recipe.AddIngredient(mod.ItemType("thunderShard"), 100);
			recipe.AddIngredient(mod.ItemType("twilightShard"), 100);
			recipe.AddIngredient(mod.ItemType("Orichalchum"));
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
