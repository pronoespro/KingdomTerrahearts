using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
    public class roxasPants:ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Roxas' Jacket");
			Tooltip.SetDefault("The jacket Roxas used in a simulation" +
				"\nGreat emotional value");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.vanity = true;
		}
	}
}
