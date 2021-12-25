using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_Christmas : KeybladeBase
    {


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Decesive Pumpking");
			Tooltip.SetDefault("A keyblade made out of the christmas spirit..." +
				"\nMaybe");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 50;
			Item.width = 50;
			Item.height = 50;
			Item.scale = 0.9f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;

			SaveAtributes();
			magic = keyMagic.ice;
			keyTransformations = new keyTransformation[] {keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Transformations/Christmas_Gun", "Items/Weapons/Transformations/Christmas_Cannon" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 3, 20 };
			keyLevel = 1;
			keybladeElement = keyType.fire;
			comboMax = 5;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FrostCore);
			recipe.AddIngredient(ItemID.IronFence,5);
			recipe.AddIngredient(ModContent.ItemType<Materials.frostStone>(),10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override void ChangeKeybladeValues()
		{
			magic = keyMagic.ice;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Transformations/Christmas_Gun", "Items/Weapons/Transformations/Christmas_Cannon" };
			formChanges = new keyDriveForm[] { keyDriveForm.element , keyDriveForm.element };
			animationTimes = new int[] { 20,1, 20 };
			keybladeElement = keyType.fire;
			comboMax = 5;
			transProj = new int[] { ProjectileID.Flames, ProjectileID.Present };
			transShootSpeed = new float[] { 7 };
			transSounds = new Terraria.Audio.LegacySoundStyle[] { SoundID.Item1, SoundID.Item19,SoundID.Item19 };
		}
		
	}
}
