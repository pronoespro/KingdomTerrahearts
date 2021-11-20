using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Placeable
{
    class TwilightWorkbench:ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twiligth Night Table");
			Tooltip.SetDefault("Night Table that you can use as a workbench.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 14;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile=ModContent.TileType<Tiles.TwilightWorkbench>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<Placeable.twilightBlock>(), 10)
			.Register();
		}

	}
}
