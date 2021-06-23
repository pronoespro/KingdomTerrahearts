using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_whishes : Keyblade
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Three wishes");
			Tooltip.SetDefault("Its powers will bring you great treasure");
		}

		public override void SetDefaults()
		{
			item.damage = 70;
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
			item.useAnimation = item.useTime = 15;

			SaveAtributes();
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.none, keyTransformation.dual };
			transSprites = new string[] { "Items/Weapons/Keyblade_oblivion", "Items/Weapons/Transformations/Keyblade_dual" };
			formChanges = new keyDriveForm[] { keyDriveForm.dark, keyDriveForm.dual };
			animationTimes = new int[] { 15, 10, 8 };
			keyLevel = 1;
			keySummon = summonType.genie;
			projectileTime = 500;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DjinnLamp);
			recipe.AddIngredient(ItemID.GoldCoin,20);
			recipe.AddIngredient(ItemID.Shackle);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 4;
			keySummon = summonType.genie;
			projectileTime = 500;
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.none, keyTransformation.dual };
			transSprites = new string[] { "Items/Weapons/Keyblade_oblivion", "Items/Weapons/Transformations/Keyblade_dual" };
			formChanges = new keyDriveForm[] { keyDriveForm.dark, keyDriveForm.dual };
			animationTimes = new int[] { 15, 10, 8 };
			keySummon = summonType.genie;
			projectileTime = 500;
		}
	}
}
