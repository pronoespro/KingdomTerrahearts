using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_destiny:Keyblade
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Destiny's Embrace");
			Tooltip.SetDefault("A keyblade made from the heart of a princess of light");
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.scale = 0.75f;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 150000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;

			SaveAtributes();
			keyLevel = 2;
			magic = keyMagic.reflect;
			magicCost = 15;
			keyTransformations = new keyTransformation[] {keyTransformation.staff };
            transSprites = new string[] { "Items/Weapons/Keyblade_destiny" };
			formChanges = new keyDriveForm[] {keyDriveForm.limit };
			animationTimes = new int[] {20,20};
			guardType = blockingType.reflect;
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
			transSprites = new string[] { "Items/Weapons/Keyblade_destiny" };
			formChanges = new keyDriveForm[] { keyDriveForm.limit };
			animationTimes = new int[] { 20, 20 };
			guardType = blockingType.reflect;
			keySummon = summonType.bambi;
		}
	}
}
