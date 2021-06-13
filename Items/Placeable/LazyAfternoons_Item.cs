using Terraria.ModLoader;
using Terraria.ID;

namespace KingdomTerrahearts.Items.Placeable
{
    public class LazyAfternoons_Item:ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Lazy Afternoons)");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("LazyAfternoons_MusicBox");
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}

    }
}
