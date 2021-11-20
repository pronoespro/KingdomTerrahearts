using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
    public class woodenSword:ComboWeaponBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wood Sword");
			Tooltip.SetDefault("A toy sword made from wood");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Melee;
			Item.width = 26;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<Keyblade_woodenStick>(), 2)
			.AddTile(TileID.WorkBenches)
			.Register();

			CreateRecipe()
			.AddIngredient(ItemID.Wood)
			.AddTile(TileID.WorkBenches)
			.Register();
		}

    }
}
