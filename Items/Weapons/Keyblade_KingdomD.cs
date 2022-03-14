using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
	public class Keyblade_KingdomD : KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kingdom key D");
			Tooltip.SetDefault("A keyblade made from your heart");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 45;
			Item.width = 80;
			Item.height = 80;
			Item.scale = 1f;
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
			keyLevel = 3;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			transSprites = new string[] { "Items/Weapons/Keyblade_KingdomD" };
			formChanges = new keyDriveForm[] { keyDriveForm.light };
			animationTimes = new int[] { 16, 7 };
		}

		public override void ChangeKeybladeValues()
		{
			magic = keyMagic.stop;
			keybladeElement = keyType.light;
			comboMax = 4;
			projectileTime = 10000;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			transSprites = new string[] { "Items/Weapons/Keyblade_KingdomD" };
			formChanges = new keyDriveForm[] { keyDriveForm.light };
			animationTimes = new int[] { 16, 7 };
			magicCost = 75;
		}

	}
}
