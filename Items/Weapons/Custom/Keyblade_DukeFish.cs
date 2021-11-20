using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Custom
{
    public class Keyblade_DukeFish: KeybladeBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Duke's Boublefang");
			Tooltip.SetDefault("Fangs, sharknados and sharks included");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 420;
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
			Item.useAnimation = Item.useTime = 20;

			SaveAtributes();
			keyLevel = 4;
			magic = keyMagic.wind;
			projectileTime *= 5;
			keyTransformations = new keyTransformation[] { keyTransformation.staff, keyTransformation.shield };
			transSprites = new string[] { "Items/Weapons/Custom/Transformations/Duke_Staff", "Items/Weapons/Custom/Transformations/Duke_Shield" };
			formChanges = new keyDriveForm[] { keyDriveForm.element };
			animationTimes = new int[] { 7, 15, 25 };
			keySummon = summonType.dumbo;
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.dark;
			comboMax = 5;
			keySummon = summonType.dumbo;
			magic = keyMagic.wind;
			projectileTime *= 5;
			keyTransformations = new keyTransformation[] { keyTransformation.staff, keyTransformation.shield };
			transSprites = new string[] { "Items/Weapons/Custom/Transformations/Duke_Staff", "Items/Weapons/Custom/Transformations/Duke_Shield" };
			formChanges = new keyDriveForm[] { keyDriveForm.element };
			animationTimes = new int[] { 7, 15, 25 };
			keySummon = summonType.dumbo;
		}

	}
}
