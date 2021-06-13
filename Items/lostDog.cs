using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    class lostDog : ModItem
	{
		Color[] itemNameCycleColors = new Color[]{
			new Color(254, 105, 47),
			new Color(190, 30, 209),
			new Color(34, 221, 151),
			new Color(0, 106, 185)
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Dog");
			Tooltip.SetDefault("Summons a dog" +
				"\nI hope someone is taking good care of you..." +
				"\nwherever you are.");
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shoot = mod.ProjectileType("zafi");
			item.width = 16;
			item.height = 30;
			item.UseSound = SoundID.Item2;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = ItemRarityID.Lime;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 5, 50, 0);
			item.buffType = mod.BuffType("zafiBuff");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("foodBowl"));
			recipe.AddIngredient(ItemID.Book);
			recipe.AddIngredient(ItemID.FallenStar,6);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tooltip = new TooltipLine(mod, "ModderItem", "Modder Item");
			tooltip.overrideColor = Color.LightBlue;
			if (!tooltips.Contains(tooltip))
			{
				tooltips.Insert(1,tooltip);
			}
			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.mod == mod.Name && line2.Name == "ModderItem")
				{
					float fade = Main.GameUpdateCount % 60 / 60f;
					int index = (int)(Main.GameUpdateCount / 60 % 4);
					line2.overrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[(index + 1) % 4], fade);
				}
			}
		}
	}
}
