using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Placeable
{
    public class WeaponDisplayItem : ModItem
	{

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Display of Inner Strength");
			Tooltip.SetDefault("Placeholder Text");
		}

		public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 13;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<Tiles.WeaponDisplay>();
            Item.placeStyle = 0;
            Item.consumable = true;
        }

	}
}
