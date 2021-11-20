using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
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
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.twilightTownBlock>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(99)
			.AddIngredient(ItemID.GrayBrick, 10)
			.AddIngredient(ModContent.ItemType<Materials.twilightShard>())
			.AddTile(TileID.Furnaces)
			.Register();
		}

	}
}
