using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_FinalXion:Keyblade
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Xion's Tears");
			Tooltip.SetDefault("A Keyblade that is reminiscent of Xion's final fight");
		}

		public override void SetDefaults()
		{
			item.damage = 2000;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 7;
			item.value = 1000000;
			item.rare = ItemRarityID.Quest;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation = item.useTime = 10;

			SaveAtributes();
			guardType = blockingType.normal;
			projectileTime = 120;
			magic = keyMagic.magnet;
			keyTransformations = new keyTransformation[] { keyTransformation.dual };
			formChanges = new keyDriveForm[] { keyDriveForm.second };
			transSprites = new string[] { "Items/Weapons/Transformations/Keyblade_FinalXionDual", "Items/Weapons/Keyblade_destiny" };
			animationTimes = new int[] { 10, 10, 15 };
			keyLevel = 100;
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 6;
			guardType = blockingType.normal;
			projectileTime = 120;
			magic = keyMagic.magnet;
			keyTransformations = new keyTransformation[] { keyTransformation.dual };
			formChanges = new keyDriveForm[] { keyDriveForm.second };
			transSprites = new string[] { "Items/Weapons/Transformations/Keyblade_FinalXionDual", "Items/Weapons/Keyblade_destiny" };
			animationTimes = new int[] { 10, 10, 15 };
		}

	}
}
