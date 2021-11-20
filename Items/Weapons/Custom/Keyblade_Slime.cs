using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Custom
{
    public class Keyblade_Slime:KeybladeBase
    {

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Ninja Slime's Blade");
			Tooltip.SetDefault("Slimming-quick teleport? Razor-sharp spikes? Slimy clones?" +
				"\nOh, yeah, that's a ninja");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 35;
			Item.width = 50;
			Item.height = 50;
			Item.scale = 0.85f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation =Item.useTime= 20;
			
			keyLevel = 2;
			magic = keyMagic.ice;
			keyTransformations = new keyTransformation[] { keyTransformation.yoyo, keyTransformation.hammer };
			transSprites = new string[] { "Items/Weapons/Custom/Transformations/Slime_Yoyo", "Items/Weapons/Custom/Transformations/Slime_Hammer" };
			formChanges = new keyDriveForm[] { keyDriveForm.element };
			animationTimes = new int[] { 20, 10 ,30};
			SaveAtributes();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 4;
			magic = keyMagic.ice;
			keyTransformations = new keyTransformation[] { keyTransformation.yoyo, keyTransformation.hammer };
			transSprites = new string[] { "Items/Weapons/Custom/Transformations/Slime_Yoyo", "Items/Weapons/Custom/Transformations/Slime_Hammer" };
			formChanges = new keyDriveForm[] { keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 30 };
		}

	}
}
