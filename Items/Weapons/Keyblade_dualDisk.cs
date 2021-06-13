using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_dualDisk:Keyblade
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dual disk");
			Tooltip.SetDefault("A digital keyblade that summons magic disks");
		}

		public override void SetDefaults()
		{
			item.damage = 75;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.scale = 0.75f;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 100;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;

			SaveAtributes();
			keyLevel = 3;
			magic = keyMagic.confuse;
			magicCost = 3;
			keyTransformations = new keyTransformation[] { keyTransformation.none};
			formChanges = new keyDriveForm[] { keyDriveForm.wisdom};
			transSprites = new string[] { "Items/Weapons/Keyblade_dualDisk" };
			animationTimes = new int[] { 15,12};
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EnchantedBoomerang,2);
			recipe.AddIngredient(ItemID.Nanites,10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EnchantedBoomerang);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.EnchantedBoomerang,2);
			recipe.AddRecipe();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.digital;
			canShootAgain = false;
			comboMax = 4;
			projectileTime = 600;
		}

	}
}
