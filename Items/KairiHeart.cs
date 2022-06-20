using Microsoft.Xna.Framework;
using SubworldLibrary;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class KairiHeart:ModItem
    {

        Mod fargoDLC;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kairi's heart");
            Tooltip.SetDefault("The heart of a princes of light");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 20;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 40;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;

        }

        public override bool? UseItem(Player player)
        {
            SubworldSystem.Enter<Subworlds.Olympus>();
            return base.UseItem(player);
        }

    }
}
