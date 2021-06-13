using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
	public class Keyblade_umbrella : Keyblade
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Casual Gear 'Amburera'");
			Tooltip.SetDefault("This looks awfully familiar...");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
			item.melee = true;
			item.width = 26;
			item.height = 26;
			item.scale = 2f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 0;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation = item.useTime = 20;

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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Umbrella);
			recipe.AddIngredient(mod.ItemType("twilightShard"),100);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.dark;
			comboMax = 2;
			keySummon = summonType.dumbo;
		}
	}
}
