using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class roxasJacket:ModItem
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
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}

	}
}
