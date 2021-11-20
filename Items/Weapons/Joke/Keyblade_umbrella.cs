using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
	public class Keyblade_umbrella : KeybladeBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Casual Gear 'Amburera'");
			Tooltip.SetDefault("This looks awfully familiar...");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 40;
			Item.width = 26;
			Item.height = 26;
			Item.scale = 2f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;

			SaveAtributes();
			magic = keyMagic.balloon;
			magicCost = 5;
			keyTransformations = new keyTransformation[] { };
			transSprites = new string[] { };
			formChanges = new keyDriveForm[] { };
			animationTimes = new int[] { 20, 20 };
			keyLevel = 2;
			keySummon = summonType.dumbo;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.Umbrella)
			.AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(),100)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.dark;
			comboMax = 2;
			keySummon = summonType.dumbo;
			magic = keyMagic.balloon;
			magicCost = 5;
			keyTransformations = new keyTransformation[] { };
			transSprites = new string[] { };
			formChanges = new keyDriveForm[] { };
			animationTimes = new int[] { 20, 20 };
			keySummon = summonType.dumbo;
		}
	}
}
