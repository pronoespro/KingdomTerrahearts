using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Creative;
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
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.width = 16;
			Item.height = 30;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 0, 50, 0);
		}

	}
}
