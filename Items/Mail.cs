using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class Mail : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mail");
			Tooltip.SetDefault("Use this on a mailbox to get rewards" +
				"\nEvery 5 rewards you get a Special Reward" +
				"\nEvery 20 rewards you get an Amazing Reward!");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
            Item.height = 16;
			Item.maxStack = 99;
            Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 5, 50, 0);
		}

	}
}
