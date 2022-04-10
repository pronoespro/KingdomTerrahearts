using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_FinalXion:KeybladeBase
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Xion's Tears");
			Tooltip.SetDefault("A Keyblade that is reminiscent of Xion's final fight");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 2000;
			Item.width = 50;
			Item.height = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 7;
			Item.value = 1000000;
			Item.rare = ItemRarityID.Quest;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 10;

			SaveAtributes();
			guardType = keybladeBlockingType.reversal;
			projectileTime = 120;
			magic = keyMagic.magnet;
			keyTransformations = new keyTransformation[] { keyTransformation.dual };
			formChanges = new keyDriveForm[] { keyDriveForm.limit };
			transSprites = new string[] { "Items/Weapons/Transformations/Keyblade_FinalXionDual"};
			animationTimes = new int[] { 10, 20 };
			keyLevel = 100;
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 6;
			guardType = keybladeBlockingType.reversal;
			projectileTime = 120;
			magic = keyMagic.magnet;
			keyTransformations = new keyTransformation[] { keyTransformation.dual };
			formChanges = new keyDriveForm[] { keyDriveForm.second };
			transSprites = new string[] { "Items/Weapons/Transformations/Keyblade_FinalXionDual"};
			animationTimes = new int[] { 10, 20 };
		}

	}
}
