using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
	public class Keyblade_Kingdom :Keyblade
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kingdom key");
			Tooltip.SetDefault("A keyblade made from your heart");
		}

		public override void SetDefaults()
		{
			item.damage = 35;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.scale = 0.75f;
			item.useTime = 16;
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
			recipe.AddIngredient(ItemID.Arkhalis);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EnchantedSword);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			keybladeElement = keyType.light;
			comboMax = 4;
			projectileTime = 10000;
			return base.CanUseItem(player);
		}

	}
}
