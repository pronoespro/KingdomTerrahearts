using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    [AutoloadEquip(EquipType.Wings)]
    public class fairyDust:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fairy Dust");
            Tooltip.SetDefault("All it takes is faith and trust..." +
                "\nOh! And something I forgot: Dust" +
                "\nJust a little bit of pixie dust");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(1000000000, 13f, 4f);
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 50;
            Item.height = 50;
            Item.scale = 0.1f;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 100;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item79;
            Item.noMelee = true;
        }

        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (Main.rand.Next(2) <= 1)
            {
                Dust.NewDust(player.Center, 0, 0, DustID.BlueFairy, Main.rand.NextFloat(0f, 1f) + player.velocity.X, Main.rand.NextFloat(-1f, 1f) + player.velocity.Y, Scale: 0.5f);
            }
            return base.UseItem(player);
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0f; // Falling glide speed
            ascentWhenRising = 0.5f; // Rising speed
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 7f;
            constantAscend = 0.2f;
        }

        public override void EquipFrameEffects(Player player, EquipType type)
        {
            base.EquipFrameEffects(player, type);

            if (Math.Abs(player.velocity.Y) > 1)
            {
                if (Main.rand.Next(2) <= 1)
                {
                    Dust.NewDust(player.position, player.width, player.height, DustID.BlueFairy, -player.velocity.X/2, -player.velocity.Y/2,Scale:0.5f);
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.PixieDust, 50)
            .AddIngredient(ItemID.FallenStar,3)
            .AddIngredient(ItemID.SoulofFlight,137)
            .AddTile(TileID.WorkBenches)
            .Register();
        }

    }
}
