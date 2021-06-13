using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Custom
{
    public class Keyblade_Slime:Keyblade
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ninja Slime's Blade");
			Tooltip.SetDefault("Slimming-quick teleport? Razor-sharp spikes? Slimy clones?" +
				"\nOh, yeah, that's a ninja");
		}

		public override void SetDefaults()
		{
			item.damage = 35;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.scale = 0.85f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 100;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation =item.useTime= 20;
			
			keyLevel = 2;
			magic = keyMagic.ice;
			keyTransformations = new keyTransformation[] { keyTransformation.yoyo, keyTransformation.hammer };
			transSprites = new string[] { "Items/Weapons/Keyblade_demonite", "Items/Weapons/Keyblade_destiny" };
			formChanges = new keyDriveForm[] { keyDriveForm.element };
			animationTimes = new int[] { 20, 10 ,30};
			SaveAtributes();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 4;
		}

	}
}
