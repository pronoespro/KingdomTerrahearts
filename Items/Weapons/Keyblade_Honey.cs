using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_Honey : KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hunny Spout");
			Tooltip.SetDefault("You're braver than you believe, and stronger than you seem, and smarter than you think");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 26;
			Item.width = Item.height = 50;
			Item.scale = 0.85f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;

			SaveAtributes();
			magic = keyMagic.balloon;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Transformations/Honey_Gun", "Items/Weapons/Transformations/Honey_Cannon" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 35 };
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
			transSprites = new string[] { "Items/Weapons/Transformations/Honey_Gun", "Items/Weapons/Transformations/Honey_Cannon" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 13, 75 };
            transProj =new int[]{ ProjectileID.Bee,ProjectileID.Beenade};
			projectileTime = 1000;
		}
	}
}
