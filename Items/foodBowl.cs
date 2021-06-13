using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class foodBowl:ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dog Food");
			Tooltip.SetDefault("A bowl of dog food" +
				"\nUsed to craft the ultimate pet ever" +
				"\nPlease come home...");
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.width = 16;
			item.height = 30;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(0, 0, 50, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bone, 10);
			recipe.AddIngredient(ItemID.Bowl);
			recipe.AddIngredient(ItemID.RottenChunk,3);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}
