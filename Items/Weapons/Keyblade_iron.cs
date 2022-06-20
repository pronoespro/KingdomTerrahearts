using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_iron:KeybladeBase
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight keyblade");
			Tooltip.SetDefault("A keyblade for common keyblade wielders");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 17;
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
			Item.useTime = Item.useAnimation = 20;

			SaveAtributes();
			keyLevel = 1;
			keyTransformations = new keyTransformation[0];
			transSprites = new string[] { };
			formChanges = new keyDriveForm[] { keyDriveForm.second };
			animationTimes = new int[] { 20, 20 };
			keySummon = summonType.mewwow;
			comboMax = 3;
			keybladeElement = keyType.light;
		}

		public override void ChangeKeybladeValues()
		{
			keyTransformations = new keyTransformation[0];
			transSprites = new string[] { };
			formChanges = new keyDriveForm[] { keyDriveForm.second };
			animationTimes = new int[] { 20, 20 };
			keySummon = summonType.mewwow;
			comboMax = 3;
			keybladeElement = keyType.light;
		}

		public override void AddRecipes()
		{

			//iron
			CreateRecipe()
			.AddIngredient(ItemID.IronBar, 10)
			.AddIngredient(ItemID.Ruby,1)
			.AddTile(TileID.Anvils)
			.Register();

			CreateRecipe()
			.AddIngredient(ItemID.IronBar, 10)
			.AddIngredient(ItemID.FallenStar, 5)
			.AddTile(TileID.Anvils)
			.Register();

			//lead

			CreateRecipe()
			.AddIngredient(ItemID.LeadBar, 10)
			.AddIngredient(ItemID.Ruby, 1)
			.AddTile(TileID.Anvils)
			.Register();

			CreateRecipe()
			.AddIngredient(ItemID.LeadBar, 10)
			.AddIngredient(ItemID.FallenStar, 5)
			.AddTile(TileID.Anvils)
			.Register();
		}

	}
}
