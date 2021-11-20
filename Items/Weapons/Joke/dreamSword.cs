using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
    public class dreamSword:ComboWeaponBase
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dream Sword");
			Tooltip.SetDefault("The power of the warior\nInvincible courage\nA sword of terrible destruction");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 25;
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
			canKill = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.FallenStar)
			.AddTile(TileID.WorkBenches)
			.Register();
		}


	}
}
