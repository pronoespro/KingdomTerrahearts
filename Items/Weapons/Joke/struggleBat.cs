using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
    public class struggleBat: ComboWeaponBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Struggle bat");
			Tooltip.SetDefault("LET's... STRUGGLE!");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType= DamageClass.Melee;
			Item.width = 26;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 15;
			Item.scale = 1.5f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.Wood)
			.AddIngredient(ItemID.Gel,2)
			.AddTile(TileID.WorkBenches)
			.Register();
		}

	}
}
