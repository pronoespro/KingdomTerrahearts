using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_demonite:Keyblade
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darkgnaw");
			Tooltip.SetDefault("A keyblade made out of darkness");
		}

		public override void SetDefaults()
		{
			item.damage = 15;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.scale = 0.75f;
			item.useStyle = ItemUseStyleID.SwingThrow;
            item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 100;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation = item.useTime = 20;

			SaveAtributes();
			magic = keyMagic.poison;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			transSprites = new string[] { "Items/Weapons/Keyblade_destiny" };
			formChanges = new keyDriveForm[] { keyDriveForm.rage };
			animationTimes = new int[] { 20, 10 };
			keyLevel = 1;
			keybladeElement = keyType.dark;
			comboMax = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 15);
			recipe.AddIngredient(ItemID.Amethyst);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override void ChangeKeybladeValues()
		{
			magic = keyMagic.poison;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			transSprites = new string[] { "Items/Weapons/Keyblade_destiny" };
			formChanges = new keyDriveForm[] { keyDriveForm.rage };
			animationTimes = new int[] { 20, 10 };
			keybladeElement = keyType.dark;
			comboMax = 4;
		}
	}
}
