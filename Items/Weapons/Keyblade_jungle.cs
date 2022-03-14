using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_jungle:KeybladeBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle king");
			Tooltip.SetDefault("A keyblade made with the heart of nature");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 25;
			Item.width = 80;
			Item.height = 80;
			Item.scale = 0.75f;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			SaveAtributes();
			keyLevel = 2;
			keyTransformations = new keyTransformation[] { keyTransformation.spear, keyTransformation.drill};
			transSprites = new string[] { "Items/Weapons/Transformations/Jungle_Drill", "Items/Weapons/Transformations/Jungle_Drill" };
			formChanges = new keyDriveForm[] { keyDriveForm.blitz};
			animationTimes = new int[] { 20,20,15};
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.jungle;
			comboMax = 4;
			projectileTime = 60 * 5;
			keyTransformations = new keyTransformation[] {keyTransformation.spear, keyTransformation.drill };
			transSprites = new string[] { "Items/Weapons/Transformations/Jungle_Drill", "Items/Weapons/Transformations/Jungle_Drill" };
			formChanges = new keyDriveForm[] { keyDriveForm.blitz };
			animationTimes = new int[] { 20,30, 15 };
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.Stinger, 12)
			.AddIngredient(ItemID.JungleSpores, 12)
			.AddIngredient(ItemID.Vine)
			.AddTile(TileID.Anvils)
			.Register();
		}
	}
}
