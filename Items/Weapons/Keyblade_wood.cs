using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
	public class Keyblade_wood : KeybladeBase
	{

		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Toy Keyblade");
			Tooltip.SetDefault("A keyblade made out of wood" +
				"\nvery basic");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 7;
			Item.width = 50;
			Item.height = 50;
			Item.scale = 0.75f;
			Item.useTime = 30;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			SaveAtributes();
			keyLevel = 0;
			guardType = blockingType.none;
			canShootAgain = true;
			keyTransformations = new keyTransformation[] { };
			formChanges = new keyDriveForm[] { };
			animationTimes = new int[] {30};
			keySummon = summonType.mushu;
		}

		public override void AddRecipes() 
		{
			CreateRecipe()
			.AddIngredient(ItemID.Wood, 10)
			.AddTile(TileID.WorkBenches)
			.Register();
		}

		public override void ChangeKeybladeValues()
		{
			canShootAgain = true;
			manaConsumed = 1;
			keybladeElement = keyType.light;
			comboMax = 4;
			projectileTime = 10;
			keyComboType = KeyComboType.normal;
			keySummon = summonType.mushu;
			keyTransformations = new keyTransformation[] { };
			formChanges = new keyDriveForm[] { };
			animationTimes = new int[] { 30 };
		}
	}
}