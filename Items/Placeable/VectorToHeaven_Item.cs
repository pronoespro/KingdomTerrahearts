using Terraria.ModLoader;
using Terraria.ID;

namespace KingdomTerrahearts.Items.Placeable
{
    class VectorToHeaven_Item:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Vector to Heaven)");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("VectorToHeaven_MusicBox");
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}
	}
}
