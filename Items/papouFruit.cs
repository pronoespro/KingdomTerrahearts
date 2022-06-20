using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class papouFruit:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Papou Fruit");
            Tooltip.SetDefault("It is said that if two people share this fruit" +
                "\ntheir fates will become forever intertwined" +
                "\n..." +
                "\nWhat about three people?");
        }

        public override void SetDefaults()
        {

            Item.consumable = true;
            Item.width = 50;
            Item.height = 50;
            Item.scale = 0.5f;
            Item.rare = ItemRarityID.Lime;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.holdStyle = ItemHoldStyleID.HoldFront;

        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation.X -= player.direction * 25;
            player.itemLocation.Y +=10;
        }

        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            player.AddBuff(BuffID.WellFed3,1000000000,foodHack:true);
            return base.UseItem(player);
        }


    }
}
