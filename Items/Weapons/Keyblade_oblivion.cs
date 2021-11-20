using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_oblivion: KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oblivion");
			Tooltip.SetDefault("A Keyblade black as darkness");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 45;
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
			magic = keyMagic.reflect;
			keyTransformations = new keyTransformation[] { keyTransformation.none,keyTransformation.dual };
			transSprites = new string[] { "Items/Weapons/Keyblade_oblivion", "Items/Weapons/Transformations/Keyblade_dual" };
			formChanges = new keyDriveForm[] { keyDriveForm.dark,keyDriveForm.dual };
			animationTimes = new int[] { 15, 10, 20 };
			projectileTime = (int)(projectileTime * 3.5f);
			keyLevel = 1;
			keySummon = summonType.dualKeys;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SoulofNight, 13)
			.AddIngredient(ItemID.DarkShard, 13)
			.AddIngredient(ItemID.MythrilBar, 13)
			.AddIngredient(ItemID.LightsBane)
			.AddTile(TileID.MythrilAnvil).Register();

			CreateRecipe().AddIngredient(ItemID.SoulofNight, 13)
			.AddIngredient(ItemID.DarkShard,13)
			.AddIngredient(ItemID.OrichalcumBar, 13)
			.AddIngredient(ItemID.LightsBane)
			.AddTile(TileID.MythrilAnvil)
			.Register();

		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 4;
			keySummon = summonType.dualKeys;
			magic = keyMagic.reflect;
			keyTransformations = new keyTransformation[] { keyTransformation.none, keyTransformation.dual };
			transSprites = new string[] { "Items/Weapons/Keyblade_oblivion", "Items/Weapons/Transformations/Keyblade_dual" };
			formChanges = new keyDriveForm[] { keyDriveForm.dark, keyDriveForm.dual };
			animationTimes = new int[] { 15, 10, 20 };
			projectileTime = (int)(projectileTime * 3.5f);
			keySummon = summonType.dualKeys;
		}
	}
}
