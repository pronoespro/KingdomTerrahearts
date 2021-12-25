using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_Eye:KeybladeBase
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Gazing Eye");
			Tooltip.SetDefault("A keyblade that has an eye in it");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 1500;
			Item.width = 50;
			Item.height = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 10;

			SaveAtributes();
			keyLevel = 6;
			projectileTime *= 6;
			magic = keyMagic.ice;
			guardType = blockingType.reflect;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			formChanges = new keyDriveForm[] { keyDriveForm.rage };
			transSprites = new string[] { "Items/Weapons/Keyblade_Eye"};
			animationTimes = new int[] { 10, 10 };
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.LunarBar, 15)
			.AddIngredient(ModContent.ItemType<Items.Materials.OrichalchumPlus>())
			.AddIngredient(ItemID.Sapphire)
			.AddIngredient(ItemID.SoulofNight,15)
			.AddIngredient(ItemID.SoulofMight, 15)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.dark;
			magic = keyMagic.ice;
			guardType = blockingType.reflect;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			formChanges = new keyDriveForm[] { keyDriveForm.rage };
			transSprites = new string[] { "Items/Weapons/Keyblade_Eye" };
			animationTimes = new int[] { 10, 10 };
		}

	}
}
