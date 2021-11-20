using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_witchDoctor:KeybladeBase
    {


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Circle of life");
			Tooltip.SetDefault("A keyblade made by an animal doctor");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 32;
			Item.width = 80;
			Item.height = 80;
			Item.scale = 0.75f;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			SaveAtributes();
			keyLevel = 2;
			keyTransformations = new keyTransformation[] {keyTransformation.yoyo };
			transSprites = new string[] { "Items/Weapons/Transformations/witch_yoyo" };
			formChanges = new keyDriveForm[] { keyDriveForm.strike};
			animationTimes = new int[] { 24, 30};
			keySummon = summonType.simba;
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.jungle;
			comboMax = 4;
			keySummon = summonType.simba;
			keyTransformations = new keyTransformation[] { keyTransformation.yoyo };
			transSprites = new string[] { "Items/Weapons/Transformations/witch_yoyo" };
			formChanges = new keyDriveForm[] { keyDriveForm.strike };
			animationTimes = new int[] { 24, 30 };
			magic = keyMagic.poison;
		}

	}
}
