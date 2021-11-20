using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_ultima:KeybladeBase
    {


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultima weapon");
			Tooltip.SetDefault("The supreme Keyblade");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 2400;
			Item.width = 50;
			Item.height = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 5;
			Item.value = 1000000;
			Item.rare = ItemRarityID.Quest;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 10;

			SaveAtributes();
			guardType = blockingType.reversal;
			projectileTime =999;
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.swords };
			formChanges = new keyDriveForm[] { keyDriveForm.ultimate };
			transSprites = new string[] { "Items/Weapons/Transformations/Ultimate_Swords"};
			animationTimes = new int[] { 10,25 };
			keyLevel = 100;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.LunarBar, 150)
			.AddIngredient(ModContent.ItemType<Items.Materials.Orichalchum>(), 15)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 6;
			guardType = blockingType.reversal;
			projectileTime = 999;
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.swords };
			formChanges = new keyDriveForm[] { keyDriveForm.ultimate };
			transSprites = new string[] { "Items/Weapons/Transformations/Ultima_Swords" };
			animationTimes = new int[] { 10, 25 };
		}


	}
}
