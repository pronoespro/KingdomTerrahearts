using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class gummiPhone:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gummi Phone");
            Tooltip.SetDefault("Displays everything" +
                "\nAllows you to teleport to save stations(WIP)" +
                "\nAllows you to know more about the darkness of another world");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 26;
            Item.value = 100000;
            Item.rare = ItemRarityID.Yellow;
            Item.useAnimation =Item.useTime= 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item6;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.CellPhone)
            .AddIngredient(ModContent.ItemType<Materials.thunderShard>(),10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }

        public override bool? UseItem(Player player)
        {
            if (player.SpawnX > 0 && player.SpawnY > 0)
            {
                player.Teleport(new Vector2(player.SpawnX * 16, player.SpawnY * 16) - new Vector2(player.width, player.height));
            }
            else
            {
                player.Teleport(player.GetModPlayer<SoraPlayer>().originalSpawnPoint);
            }
            player.velocity = Vector2.Zero;

            return true;
        }

        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);

            player.accDepthMeter++;
            player.accCalendar = true;
            player.accStopwatch = true;
            player.accWeatherRadio = true;
            player.accOreFinder = true;
            player.accThirdEye = true;
            player.accWatch++;
            player.accCompass++;
            player.accCritterGuide = true;
            player.accDreamCatcher = true;
            player.accFishFinder = true;
            player.InfoAccMechShowWires = true;
            player.accJarOfSouls = true;

            

        }

    }
}
