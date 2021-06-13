using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_flameFrolic : Keyblade
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frolic Flame");
			Tooltip.SetDefault("A fiery keyblade for mischevious rascals");
		}

		public override void SetDefaults()
		{
			item.damage = 38;
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
			keyTransformations = new keyTransformation[] { keyTransformation.none, keyTransformation.dual };
			transSprites = new string[] { "Items/Weapons/Keyblade_oblivion", "Items/Weapons/Transformations/Keyblade_dual" };
			formChanges = new keyDriveForm[] { keyDriveForm.dark, keyDriveForm.dual };
			animationTimes = new int[] { 20, 15, 10 };
			projectileTime = (int)(projectileTime * 3.5f);
			keyLevel = 1;
			keySummon = summonType.mushu;
			SaveAtributes();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar,23);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.fire;
			comboMax = 4;
			keySummon = summonType.mushu;
		}
	}
}
