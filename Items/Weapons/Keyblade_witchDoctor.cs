using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_witchDoctor:Keyblade
    {


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Circle of life");
			Tooltip.SetDefault("A keyblade made by an animal doctor");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.scale = 0.75f;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 100;
            item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;

			SaveAtributes();
			keyLevel = 2;
			keyTransformations = new keyTransformation[] {keyTransformation.yoyo };
			transSprites = new string[] { "Items/Weapons/Keyblade_witchDoctor"};
			formChanges = new keyDriveForm[] { keyDriveForm.strike};
			animationTimes = new int[] { 24, 30};
			keySummon = summonType.simba;
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.jungle;
			comboMax = 4;
			keySummon = summonType.simba;
		}

	}
}
