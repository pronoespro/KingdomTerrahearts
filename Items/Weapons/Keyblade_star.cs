using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_star: Keyblade
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
			Tooltip.SetDefault("A keyblade made out of magic");
		}

		public override void SetDefaults()
		{
			item.damage = 15;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.scale = 0.75f;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 100;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;

			SaveAtributes();
			keyLevel = 1;
			magic = keyMagic.magnet;
			keyTransformations = new keyTransformation[] {keyTransformation.guns,keyTransformation.cannon };
			transSprites = new string[]{ "Items/Weapons/sharpshooter", "Items/Weapons/sharpshooter" };
			formChanges = new keyDriveForm[] { keyDriveForm.element,keyDriveForm.element};
			animationTimes = new int[] { 20,10,30};
			keySummon = summonType.chickenLittle;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 6);
			recipe.AddIngredient(ItemID.ManaCrystal,2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.star;
			comboMax = 4;
			keySummon = summonType.chickenLittle;
			magic = keyMagic.magnet;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/sharpshooter", "Items/Weapons/sharpshooter" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 30 };
			keySummon = summonType.chickenLittle;
		}
	}
}
