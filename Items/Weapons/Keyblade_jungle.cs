﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_jungle:Keyblade
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle king");
			Tooltip.SetDefault("A keyblade made with the heart of nature");
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.scale = 0.75f;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 100;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;

			SaveAtributes();
			keyLevel = 2;
			keyTransformations = new keyTransformation[] { keyTransformation.drill};
			transSprites = new string[] { "Items/Weapons/Keyblade_jungle" };
			formChanges = new keyDriveForm[] { keyDriveForm.blitz};
			animationTimes = new int[] { 20,15};
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.jungle;
			comboMax = 4;
			projectileTime = 60 * 5;
			keyTransformations = new keyTransformation[] { keyTransformation.drill };
			transSprites = new string[] { "Items/Weapons/Keyblade_jungle" };
			formChanges = new keyDriveForm[] { keyDriveForm.blitz };
			animationTimes = new int[] { 20, 15 };
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Stinger, 12);
			recipe.AddIngredient(ItemID.JungleSpores, 12);
			recipe.AddIngredient(ItemID.Vine);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
