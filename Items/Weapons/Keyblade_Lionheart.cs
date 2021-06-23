using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_Lionheart : Keyblade
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lionheart");
			Tooltip.SetDefault("Its powers assist those who seek supreme speed");
		}

		public override void SetDefaults()
		{
			item.damage = 24;
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

			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Keyblade_oblivion", "Items/Weapons/Keyblade_oblivion" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 15 };
			projectileTime = 1000;
			keyLevel = 1;
			keySummon = summonType.mushu;
			SaveAtributes();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.fire;
			comboMax = 4;
			keySummon = summonType.mushu;
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Keyblade_oblivion", "Items/Weapons/Keyblade_oblivion" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 15 };
			projectileTime = 1000;
			keySummon = summonType.mushu;
		}

        public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar,10);
			recipe.AddIngredient(ItemID.PhoenixBlaster);
			recipe.AddIngredient(ItemID.Torch, 25);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBar, 10);
			recipe.AddIngredient(ItemID.PhoenixBlaster);
			recipe.AddIngredient(ItemID.Torch, 25);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}


    }
}
