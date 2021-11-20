using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class Keyblade_Lionheart : KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lionheart");
			Tooltip.SetDefault("Its powers assist those who seek supreme speed");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 24;
			Item.width = Item.height = 50;
			Item.scale = 0.85f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;

			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Transformations/Lionheart_Gun", "Items/Weapons/Transformations/Lionheart_Cannon" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 55 };
			projectileTime = 1000;
			keyLevel = 1;
			keySummon = summonType.mushu;
			SaveAtributes();
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.fire;
			comboMax = 4;
			keySummon = summonType.mushu;
			magic = keyMagic.fire;
			keyTransformations = new keyTransformation[] { keyTransformation.guns, keyTransformation.cannon };
			transSprites = new string[] { "Items/Weapons/Transformations/Lionheart_Gun", "Items/Weapons/Transformations/Lionheart_Cannon" };
			formChanges = new keyDriveForm[] { keyDriveForm.element, keyDriveForm.element };
			animationTimes = new int[] { 20, 10, 55 };
			projectileTime = 1000;
			keySummon = summonType.mushu;
		}

        public override void AddRecipes()
        {
			CreateRecipe()
			.AddIngredient(ItemID.IronBar,10)
			.AddIngredient(ItemID.PhoenixBlaster)
			.AddIngredient(ItemID.Torch, 25)
			.AddTile(TileID.Anvils)
			.Register();

			CreateRecipe()
			.AddIngredient(ItemID.LeadBar, 10)
			.AddIngredient(ItemID.PhoenixBlaster)
			.AddIngredient(ItemID.Torch, 25)
			.AddTile(TileID.Anvils)
			.Register();
		}


    }
}
