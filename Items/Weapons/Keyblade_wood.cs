using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
	public class Keyblade_wood : Keyblade
	{

		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Toy Keyblade");
			Tooltip.SetDefault("A keyblade made out of wood" +
				"\nvery basic");
		}

		public override void SetDefaults() 
		{
			item.damage = 7;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.scale = 0.75f;
			item.useTime = 30;
			item.useAnimation = 16;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3;
			item.value = 100;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void ChangeKeybladeValues()
		{
			canShootAgain = false;
			manaConsumed = 1;
			keybladeElement = keyType.light;
			comboMax = 4;
			projectileTime = 1;
			keyComboType = KeyComboType.normal;
		}
	}
}