using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_star: KeybladeBase
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
			Tooltip.SetDefault("A keyblade made out of magic");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 15;
			Item.width = 50;
			Item.height = 50;
			Item.scale = 0.75f;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			SaveAtributes();
			keyLevel = 1;
			magic = keyMagic.magnet;
			keyTransformations = new keyTransformation[] {keyTransformation.guns,keyTransformation.cannon };
			transSprites = new string[]{ "Items/Weapons/sharpshooter", "Items/Weapons/sharpshooter" };
			formChanges = new keyDriveForm[] { keyDriveForm.element,keyDriveForm.element};
			animationTimes = new int[] { 20,10,30};
			keySummon = summonType.chickenLittle;
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.star;
			comboMax = 4;
			keySummon = summonType.chickenLittle;
			magic = keyMagic.magnet;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Transformations/Star_Gun", "Items/Weapons/Transformations/Star_Cannon" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 30 };
			keySummon = summonType.chickenLittle;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.FallenStar, 6)
			.AddIngredient(ItemID.ManaCrystal,2)
			.AddTile(TileID.Anvils)
			.Register();
		}
	}
}
