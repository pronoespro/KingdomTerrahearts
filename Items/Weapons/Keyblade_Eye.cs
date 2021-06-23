using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_Eye:Keyblade
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Gazing Eye");
			Tooltip.SetDefault("A keyblade that has an eye in it");
		}

		public override void SetDefaults()
		{
			item.damage = 1500;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 100;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation = item.useTime = 10;

			SaveAtributes();
			keyLevel = 6;
			projectileTime *= 6;
			magic = keyMagic.ice;
			guardType = blockingType.reflect;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			formChanges = new keyDriveForm[] { keyDriveForm.rage };
			transSprites = new string[] { "Items/Weapons/Keyblade_demonite", "Items/Weapons/Keyblade_destiny" };
			animationTimes = new int[] { 10, 10 };
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 15);
			recipe.AddIngredient(mod.ItemType("Orichalchum"));
			recipe.AddIngredient(ItemID.Sapphire);
			recipe.AddIngredient(ItemID.SoulofNight,15);
			recipe.AddIngredient(ItemID.SoulofMight, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.dark;
			magic = keyMagic.ice;
			guardType = blockingType.reflect;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			formChanges = new keyDriveForm[] { keyDriveForm.rage };
			transSprites = new string[] { "Items/Weapons/Keyblade_demonite", "Items/Weapons/Keyblade_destiny" };
			animationTimes = new int[] { 10, 10 };
		}

	}
}
