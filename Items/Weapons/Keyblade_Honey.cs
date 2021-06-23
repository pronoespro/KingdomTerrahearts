using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_Honey : Keyblade
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hunny Spout");
			Tooltip.SetDefault("You're braver than you believe, and stronger than you seem, and smarter than you think");
		}

		public override void SetDefaults()
		{
			item.damage = 26;
			item.melee = true;
			item.width = item.height = 50;
			item.scale = 0.85f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 100;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation = item.useTime = 20;

			SaveAtributes();
			magic = keyMagic.balloon;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Keyblade_oblivion", "Items/Weapons/Keyblade_oblivion" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 15 };
			projectileTime = 1000;
			keyLevel = 1;
			keySummon = summonType.bambi;
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.honey;
			comboMax = 4;
			keySummon = summonType.bambi;
			magic = keyMagic.balloon;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Keyblade_oblivion", "Items/Weapons/Keyblade_oblivion" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 15 };
			projectileTime = 1000;
		}
	}
}
