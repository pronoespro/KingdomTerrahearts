using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Armor
{

	public class orgCoat : ModItem
	{

		public static int orgCoatBodySlot = -1;
		public static int orgCoatHeadSlot = -1;
		public static int orgCoatLegsSlot = -1;

		bool showHood = true;
		public TooltipLine displayCoatLine;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Organization Coat");
			Tooltip.SetDefault("Wards off darkness and hides you from the organization" +
				"\nGive 20% more keyblade damage");
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

        public override bool CanRightClick()
        {
			return true;
        }

        public override void RightClick(Player player)
        {
			EntitySource_ItemUse s = new EntitySource_ItemUse(player, Item);
			int newCoatItem=Item.NewItem(s,player.getRect(), Item.type, noBroadcast: true, prefixGiven: Item.prefix, noGrabDelay: true);
			orgCoat newCoat =(orgCoat)Main.item[newCoatItem].ModItem;
			newCoat.ChangeVisibility(!showHood);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

			Item.defense = 5 + sp.CheckPlayerLevel() * 2;

			if (!hideVisual) {
				sp.curCostume = (!showHood) ? 1 : 0;
			}
			displayCoatLine = new TooltipLine(Mod, "DisplayCoat", (showHood ? "The hood is visible" : "The hood is invisible") + "\nRight click to toogle visibility");

			player.GetDamage(ModContent.GetInstance<KeybladeDamage>()) *= 1.2f;

		}

        public override void UpdateVanity(Player player)
		{
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

			sp.curCostume = (showHood) ? 1 : 0;
			displayCoatLine = new TooltipLine(Mod, "DisplayCoat", (showHood ? "The hood is visible" : "The hood is invisible") + "\nRight click to toogle visibility");
		}

        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<Materials.twilightShard>())
			.AddTile(TileID.Loom)
			.Register();
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (displayCoatLine == null)
			{
				displayCoatLine = new TooltipLine(Mod, "DisplayCoat", (showHood ? "The hood is visible" : "The hood is invisible") + "\nRight click to toogle visibility");
			}
			displayCoatLine.overrideColor = Color.LightBlue;
			if (!tooltips.Contains(displayCoatLine))
            {
				tooltips.Add(displayCoatLine);
            }
        }

        public override void UpdateInventory(Player player)
		{

			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
			Item.defense = 5 + sp.CheckPlayerLevel() * 2;

		}

		public void ChangeVisibility(bool visible)
        {
			showHood = visible;
        }

    }

}