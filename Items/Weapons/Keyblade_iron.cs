using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_iron:Keyblade
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight keyblade");
			Tooltip.SetDefault("A keyblade for common keyblade wielders");
		}

		public override void SetDefaults()
		{
			item.damage = 17;
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
			item.useTime = item.useAnimation = 20;

			SaveAtributes();
			keyLevel = 1;
			keyTransformations = new keyTransformation[0];
			formChanges = new keyDriveForm[] { keyDriveForm.second };
			animationTimes = new int[] { 20 };
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 3;
		}

		public override void AddRecipes()
		{

			//iron
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 10);
			recipe.AddIngredient(ItemID.Ruby,1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 10);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			//lead

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBar, 10);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBar, 10);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}
