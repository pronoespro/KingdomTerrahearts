using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Custom
{
    public class Keyblade_MoonLord: KeybladeBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Moon's Bane");
			Tooltip.SetDefault("Inpending doom of heart approaches" +
				"\nMoon lazer included");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 750;
			Item.width = 50;
			Item.height = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;

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
			projectileTime *= 4;
			magic = keyMagic.ice;
			keyTransformations = new keyTransformation[] { keyTransformation.staff, keyTransformation.swords };
			transSprites = new string[] { "Items/Weapons/Keyblade_demonite", "Items/Weapons/Keyblade_destiny" };
			formChanges = new keyDriveForm[] { keyDriveForm.element };
			animationTimes = new int[] { 7, 5, 7 };
		}

    }
}
