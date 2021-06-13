using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Custom
{
    public class Keyblade_MoonLord: Keyblade
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon's Bane");
			Tooltip.SetDefault("Inpending doom of heart approaches" +
				"\nMoon lazer included");
		}

		public override void SetDefaults()
		{
			item.damage = 750;
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
			item.useAnimation = item.useTime = 20;

			SaveAtributes();
			keyLevel = 5;
			projectileTime *= 4;
			magic = keyMagic.ice;
			keyTransformations = new keyTransformation[] { keyTransformation.staff, keyTransformation.swords };
			transSprites = new string[] { "Items/Weapons/Keyblade_demonite", "Items/Weapons/Keyblade_destiny" };
			formChanges = new keyDriveForm[] { keyDriveForm.element };
			animationTimes = new int[] { 7, 5, 7 };
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 4;
		}

    }
}
