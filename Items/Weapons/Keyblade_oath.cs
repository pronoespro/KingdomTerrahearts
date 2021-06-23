using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
	public class Keyblade_oath : Keyblade
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oathkeeper");
			Tooltip.SetDefault("An oath to return");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
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
			magic = keyMagic.lightning;
			keyTransformations = new keyTransformation[] { keyTransformation.none, keyTransformation.dual };
			transSprites = new string[] { "Items/Weapons/Keyblade_oath", "Items/Weapons/Transformations/Keyblade_dual" };
			formChanges = new keyDriveForm[] { keyDriveForm.light, keyDriveForm.dual };
			animationTimes = new int[] { 15, 10, 8 };
			projectileTime =(int)(projectileTime* 3.5f);
			keyLevel = 1;
			keySummon = summonType.dualKeys;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofLight, 7);
			recipe.AddIngredient(ItemID.FallenStar, 7);
			recipe.AddIngredient(ItemID.PalmWood,7);
			recipe.AddIngredient(mod.ItemType("KairiHeart"));
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 4;
			keySummon = summonType.dualKeys;
			magic = keyMagic.lightning;
			keyTransformations = new keyTransformation[] { keyTransformation.none, keyTransformation.dual };
			transSprites = new string[] { "Items/Weapons/Keyblade_oath", "Items/Weapons/Transformations/Keyblade_dual" };
			formChanges = new keyDriveForm[] { keyDriveForm.light, keyDriveForm.dual };
			animationTimes = new int[] { 15, 10, 8 };
			projectileTime = (int)(projectileTime * 3.5f);
			keySummon = summonType.dualKeys;
		}
	}
}
