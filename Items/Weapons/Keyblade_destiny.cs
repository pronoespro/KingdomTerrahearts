using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_destiny:Keyblade
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Destiny's Embrace");
			Tooltip.SetDefault("A keyblade made from the heart of a princess of light");
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.scale = 0.75f;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 150000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override bool CanUseItem(Player player)
		{
			canShootAgain = false;
			manaConsumed = 10;
			keybladeElement = keyType.destiny;
			comboMax = 4;
			keyComboType = KeyComboType.magic;
			return base.CanUseItem(player);
		}
	}
}
