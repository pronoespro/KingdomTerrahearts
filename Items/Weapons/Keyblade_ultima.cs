using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_ultima:Keyblade
    {


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultima weapon");
			Tooltip.SetDefault("The supreme Keyblade");
		}

		public override void SetDefaults()
		{
			item.damage = 2400;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 5;
			item.value = 1000000;
			item.rare = ItemRarityID.Quest;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation = item.useTime = 10;

			SaveAtributes();
			guardType = blockingType.reversal;
			projectileTime =999;
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.swords };
			formChanges = new keyDriveForm[] { keyDriveForm.ultimate };
			transSprites = new string[] { "Items/Weapons/Keyblade_demonite", "Items/Weapons/Keyblade_destiny" };
			animationTimes = new int[] { 10, 10 ,15};
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 150);
			recipe.AddIngredient(mod.ItemType("Orichalchum"), 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 6;
		}


	}
}
