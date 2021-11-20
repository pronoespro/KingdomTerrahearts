using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_demonite:KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darkgnaw");
			Tooltip.SetDefault("A keyblade made out of darkness");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 15;
			Item.width = 50;
			Item.height = 50;
			Item.scale = 0.75f;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;

			SaveAtributes();
			magic = keyMagic.poison;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			transSprites = new string[] {"Items/Weapons/Keyblade_demonite" };
			formChanges = new keyDriveForm[] { keyDriveForm.rage };
			animationTimes = new int[] { 20, 17 };
			keyLevel = 1;
			keybladeElement = keyType.dark;
			comboMax = 4;
		}

		public override void AddRecipes()
		{
			Recipe recipe= CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 15);
			recipe.AddIngredient(ItemID.Amethyst);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

        public override void ChangeKeybladeValues()
		{
			magic = keyMagic.poison;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			transSprites = new string[] { "Items/Weapons/Keyblade_demonite" };
			formChanges = new keyDriveForm[] { keyDriveForm.rage };
			animationTimes = new int[] { 20, 17 };
			keybladeElement = keyType.dark;
			comboMax = 4;
		}
	}
}
