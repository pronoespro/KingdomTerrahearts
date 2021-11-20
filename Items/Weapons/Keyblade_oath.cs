using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
	public class Keyblade_oath : KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oathkeeper");
			Tooltip.SetDefault("An oath to return");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 50;
			Item.width = Item.height = 50;
			Item.scale = 0.85f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 15;

			SaveAtributes();
			magic = keyMagic.lightning;
			keyTransformations = new keyTransformation[] { keyTransformation.none, keyTransformation.dual };
			transSprites = new string[] { "Items/Weapons/Keyblade_oath", "Items/Weapons/Transformations/Keyblade_dual" };
			formChanges = new keyDriveForm[] { keyDriveForm.light, keyDriveForm.dual };
			animationTimes = new int[] { 15, 13, 25 };
			projectileTime =(int)(projectileTime* 3.5f);
			keyLevel = 1;
			keySummon = summonType.dualKeys;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.SoulofLight, 7)
			.AddIngredient(ItemID.FallenStar, 7)
			.AddIngredient(ItemID.PalmWood,7)
			.AddIngredient(ModContent.ItemType<KairiHeart>())
			.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 5;
			keySummon = summonType.dualKeys;
			magic = keyMagic.lightning;
			keyTransformations = new keyTransformation[] { keyTransformation.none, keyTransformation.dual };
			transSprites = new string[] { "Items/Weapons/Keyblade_oath", "Items/Weapons/Transformations/Keyblade_dual" };
			formChanges = new keyDriveForm[] { keyDriveForm.light, keyDriveForm.dual };
			animationTimes = new int[] { 15, 13, 25 };
			projectileTime = (int)(projectileTime * 3.5f);
			keySummon = summonType.dualKeys;
		}
	}
}
