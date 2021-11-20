using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
	public class Keyblade_Kingdom :KeybladeBase
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kingdom key");
			Tooltip.SetDefault("A keyblade made from your heart");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 35;
			Item.width = 80;
			Item.height = 80;
			Item.scale = 0.75f;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			SaveAtributes();
			keyLevel = 2;
			keyTransformations = new keyTransformation[] { keyTransformation.none};
			transSprites = new string[] { "Items/Weapons/Keyblade_Kingdom" };
			formChanges = new keyDriveForm[] { keyDriveForm.second};
			animationTimes = new int[] { 16,13 };
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.Arkhalis)
			.AddTile(TileID.Anvils)
			.Register();

			CreateRecipe()
			.AddIngredient(ItemID.EnchantedSword)
			.AddTile(TileID.Anvils)
			.Register();

			CreateRecipe()
			.AddIngredient(ItemID.Starfury)
			.AddTile(TileID.Anvils)
			.Register();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 4;
			projectileTime = 10000;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			transSprites = new string[] { "Items/Weapons/Keyblade_Kingdom" };
			formChanges = new keyDriveForm[] { keyDriveForm.second };
			animationTimes = new int[] { 16, 13 };
		}

	}
}
