using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    class lostDog : ModItem
	{
		Color[] ItemNameCycleColors = new Color[]{
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
				"\nwherever you are." +
				"\nFixes keyblades (there's a bug that is yet to be understood, please use this pet)");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pets.zafi>();
			Item.width = 16;
			Item.height = 30;
			Item.UseSound = SoundID.Item2;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Lime;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 5, 50, 0);
			Item.buffType = ModContent.BuffType<Buffs.zafiBuff>();
		}

        public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
			return base.UseItem(player);
        }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tooltip = new TooltipLine(Mod, "ModderItem", "Modder Item");
			tooltip.overrideColor = Color.LightBlue;
			if (!tooltips.Contains(tooltip))
			{
				tooltips.Insert(1,tooltip);
			}
			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.mod == Mod.Name && line2.Name == "ModderItem")
				{
					float fade = Main.GameUpdateCount % 60 / 60f;
					int index = (int)(Main.GameUpdateCount / 60 % 4);
					line2.overrideColor = Color.Lerp(ItemNameCycleColors[index], ItemNameCycleColors[(index + 1) % 4], fade);
				}
			}
		}
	}
}
