using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_destiny:KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Destiny's Embrace");
			Tooltip.SetDefault("A keyblade made from the heart of a princess of light");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 30;
			Item.width = 80;
			Item.height = 80;
			Item.scale = 0.75f;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 150000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			SaveAtributes();
			keyLevel = 2;
			magic = keyMagic.reflect;
			magicCost = 15;
			keyTransformations = new keyTransformation[] {keyTransformation.staff };
            transSprites = new string[] { "Items/Weapons/Transformations/Kairi_Staff" };
			formChanges = new keyDriveForm[] {keyDriveForm.limit };
			animationTimes = new int[] {20,20};
			guardType = keybladeBlockingType.reflect;
			keySummon = summonType.bambi;
		}

		public override void ChangeKeybladeValues()
		{
			canShootAgain = false;
			manaConsumed = 10;
			keybladeElement = keyType.destiny;
			comboMax = 4;
			keyComboType = KeyComboType.magic;
			magic = keyMagic.reflect;
			magicCost = 15;
			keyTransformations = new keyTransformation[] { keyTransformation.staff };
			transSprites = new string[] { "Items/Weapons/Transformations/Kairi_Staff" };
			formChanges = new keyDriveForm[] { keyDriveForm.limit };
			animationTimes = new int[] { 20, 20 };
			guardType = keybladeBlockingType.reflect;
			keySummon = summonType.bambi;
		}
	}
}
