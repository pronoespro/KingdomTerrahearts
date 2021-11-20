using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace KingdomTerrahearts.Items.Placeable
{
    class TwilightWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Town Wall Fragment");
			Tooltip.SetDefault("A pice of a town far away" +
				"\nThe town is fine, this is just a lost fragment nobody cared about" +
	"\nThat nobody being Roxas");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Walls.TwilightWall>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(4)
			.AddIngredient(ModContent.ItemType<Placeable.twilightBlock>())
			.Register();
		}
	}
}
