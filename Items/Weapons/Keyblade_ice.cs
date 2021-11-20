using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace KingdomTerrahearts.Items.Weapons
{
    public class Keyblade_ice : KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Snow");
			Tooltip.SetDefault("A keyblade made out of ice and snow");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 150;
			Item.width = 50;
			Item.height = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.scale = 0.8f;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;

			SaveAtributes();
			magic = keyMagic.ice;
			keyTransformations = new keyTransformation[] { keyTransformation.claws, keyTransformation.skates };
			transSprites = new string[] { "Items/Weapons/Transformations/Ice_claws" , "Items/Weapons/Transformations/Ice_skates" };
			formChanges = new keyDriveForm[] { keyDriveForm.blitz };
			animationTimes = new int[] { 15, 40, 40 };
			keyLevel = 1;
			keybladeElement = keyType.ice;
			comboMax = 5;
		}

		public override void ChangeKeybladeValues()
		{
			magic = keyMagic.ice;
			keyTransformations = new keyTransformation[] { keyTransformation.claws, keyTransformation.skates };
			transSprites = new string[] { "Items/Weapons/Transformations/Ice_claws", "Items/Weapons/Transformations/Ice_skates" };
			formChanges = new keyDriveForm[] { keyDriveForm.blitz, keyDriveForm.blitz };
			animationTimes = new int[] { 15, 40, 40 };
			keybladeElement = keyType.ice;
			projectileTime = 1000;
			comboMax = 5;
			keySummon = summonType.dumbo;
		}
    }
}
