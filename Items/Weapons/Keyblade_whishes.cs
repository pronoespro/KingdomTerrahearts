using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_whishes : KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Three wishes");
			Tooltip.SetDefault("Its powers will bring you great treasure");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 70;
			Item.width = Item.height = 50;
			Item.scale = 0.85f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 15;

			SaveAtributes();
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.cannon, keyTransformation.staff};
			transSprites = new string[] { "Items/Weapons/Transformations/Lamp_Cannon", "Items/Weapons/Transformations/Lamp_Staff" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.wisdom};
			animationTimes = new int[] { 15, 10, 8 };
			keyLevel = 1;
			keySummon = summonType.genie;
			projectileTime = 500;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.DjinnLamp)
			.AddIngredient(ItemID.GoldCoin,20)
			.AddIngredient(ItemID.Shackle)
			.AddTile(TileID.Anvils)
			.Register();

		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.light;
			comboMax = 4;
			keySummon = summonType.genie;
			projectileTime = 500;
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.cannon, keyTransformation.staff };
			transSprites = new string[] { "Items/Weapons/Transformations/Lamp_Cannon", "Items/Weapons/Transformations/Lamp_Staff" };
			formChanges = new keyDriveForm[] { keyDriveForm.dark, keyDriveForm.dual };
			animationTimes = new int[] { 15, 10, 8 };
			keySummon = summonType.genie;
			projectileTime = 500;
		}
	}
}
