using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_dualDisk:KeybladeBase
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dual disk");
			Tooltip.SetDefault("A digital keyblade that summons magic disks");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 75;
			Item.width = 80;
			Item.height = 80;
			Item.scale = 0.75f;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

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
			CreateRecipe()
			.AddIngredient(ItemID.EnchantedBoomerang,2)
			.AddIngredient(ItemID.Nanites,10)
			.AddTile(TileID.Anvils)
			.Register();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.digital;
			canShootAgain = false;
			comboMax = 4;

			projectileTime = 600;
			magic = keyMagic.confuse;
			magicCost = 3;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			formChanges = new keyDriveForm[] { keyDriveForm.wisdom };
			transSprites = new string[] { "Items/Weapons/Keyblade_dualDisk" };
			animationTimes = new int[] { 15, 12 };
		}

	}
}
