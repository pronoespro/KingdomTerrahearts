using KingdomTerrahearts.Extra;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Armor
{

	public class orgCoat : ModItem
	{

		public static int orgCoatBodySlot = -1;
		public static int orgCoatHeadSlot = -1;
		public static int orgCoatLegsSlot = -1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Organization Coat");
			Tooltip.SetDefault("Wards off darkness and hides you from the organization" +
				"\nDuplicates keyblade damage");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.accessory = true;
			Item.value = 1500;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 5;
			Item.canBePlacedInVanityRegardlessOfConditions = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

			Item.defense = 5 + sp.CheckPlayerLevel() * 2;

			sp.curCostume = (hideVisual) ? 1 : 0;

			player.GetDamage(ModContent.GetInstance<KeybladeDamage>()) *= 2;

		}

        public override void UpdateVanity(Player player)
		{
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
			sp.curCostume = 0;
		}

        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<Materials.twilightShard>())
			.AddTile(TileID.Loom)
			.Register();
		}
	}

}