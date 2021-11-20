using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_flameFrolic : KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frolic Flame");
			Tooltip.SetDefault("A fiery keyblade for mischevious rascals");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 38;
			Item.width = Item.height = 50;
			Item.scale = 0.85f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;

			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.yoyo };
			transSprites = new string[] { "Items/Weapons/Org13/Axel/Chacrams_EternalFlames" };
			formChanges = new keyDriveForm[] { keyDriveForm.valor};
			animationTimes = new int[] { 20, 15, 10 };
			projectileTime = (int)(projectileTime * 3.5f);
			keyLevel = 1;
			keySummon = summonType.mushu;
			SaveAtributes();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.HellstoneBar,23)
			.AddTile(TileID.Anvils)
			.Register();

		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.fire;
			comboMax = 4;
			keySummon = summonType.mushu;
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.yoyo};
			transSprites = new string[] { "Items/Weapons/Org13/Axel/Chacrams_EternalFlames" };
			formChanges = new keyDriveForm[] { keyDriveForm.valor };
			animationTimes = new int[] { 20, 15, 10 };
			projectileTime = (int)(projectileTime * 3.5f);
			keySummon = summonType.mushu;
		}
	}
}
