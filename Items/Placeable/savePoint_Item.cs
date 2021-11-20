using Terraria.GameContent.Creative;
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
			Tooltip.SetDefault("A place where you can collect your thoughts" +
				"\nStay near it to recover your health and mana" +
				"\nUse to save your game, set your spawn point and skip time");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 2000;
			Item.createTile = TileType<Tiles.savepoint>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.Bed)
			.AddIngredient(ItemID.FallenStar, 15)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}
