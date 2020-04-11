using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_witchDoctor:Keyblade
    {


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Circle of life");
			Tooltip.SetDefault("A keyblade made by an animal doctor");
		}

		public override void SetDefaults()
		{
			item.damage = 17;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.scale = 0.75f;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 100;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override bool CanUseItem(Player player)
		{
			keybladeElement = keyType.jungle;
			comboMax = 4;
			return base.CanUseItem(player);
		}

	}
}
