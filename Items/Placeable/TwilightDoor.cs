using Terraria.GameContent.Creative;
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
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 28;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.TwilightDoorClosed>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<Placeable.twilightBlock>(), 15)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}
