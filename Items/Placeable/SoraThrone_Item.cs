using Terraria.GameContent.Creative;
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
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.SoraThrone>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.Throne)
			.AddIngredient(ModContent.ItemType<Materials.blazingShard>(),100)
			.AddIngredient(ModContent.ItemType<Materials.denseShard>(),100)
			.AddIngredient(ModContent.ItemType<Materials.frostShard>(),100)
			.AddIngredient(ModContent.ItemType<Materials.lucidShard>(),100)
			.AddIngredient(ModContent.ItemType<Materials.powerShard>(), 100)
			.AddIngredient(ModContent.ItemType<Materials.pulsingShard>(),100)
			.AddIngredient(ModContent.ItemType<Materials.thunderShard>(), 100)
			.AddIngredient(ModContent.ItemType<Materials.twilightShard>(), 100)
			.AddIngredient(ModContent.ItemType<Materials.Orichalchum>())
			.AddTile(TileID.Anvils)
			.Register();
		}
	}
}
