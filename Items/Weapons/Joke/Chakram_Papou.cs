using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KingdomTerrahearts.Extra;
using Terraria.ModLoader.IO;
using System;
using KingdomTerrahearts.Items.Weapons.Org13;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
    public class Chakram_Papou : ChakramBase
    {

        public override string Texture => "KingdomTerrahearts/Items/papouFruit";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Papou Fruits");
            Tooltip.SetDefault("Let me show my affection!");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 137;
            Item.height = Item.width = 50;
            Item.knockBack = 50;
            Item.maxStack = 999;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 0.75f;
            Item.shootSpeed = 50;
            Item.useAnimation = 1;
            Item.useTime = 1;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            projectiles = new int[] { ProjectileID.FruitcakeChakram, ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Papou>() };
            maxChakrams = 999;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ProjectileID.FruitcakeChakram,ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Papou>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(999)

            .AddIngredient(ModContent.ItemType<papouFruit>(), 999999)

            .AddIngredient(ItemID.FruitcakeChakram, 999999)

            .AddIngredient(ItemID.SoulofFlight, 99999)
            .AddIngredient(ItemID.SoulofFright, 99999)
            .AddIngredient(ItemID.SoulofLight, 99999)
            .AddIngredient(ItemID.SoulofMight, 99999)
            .AddIngredient(ItemID.SoulofNight, 99999)
            .AddIngredient(ItemID.SoulofSight, 99999)

            .AddIngredient(ItemID.FragmentNebula, 99999)
            .AddIngredient(ItemID.FragmentSolar, 99999)
            .AddIngredient(ItemID.FragmentStardust, 99999)
            .AddIngredient(ItemID.FragmentVortex, 99999)

            .AddIngredient(ItemID.Zenith, 999999)

            .AddIngredient(ModContent.ItemType<Keyblade_destiny>(), 999999)
            .AddIngredient(ModContent.ItemType<Keyblade_flameFrolic>(), 999999)

            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Ashes>(),999)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_BlazeOfGlory>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Burnout>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Combustion>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Conformers>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Corona>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_DelayedAction>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_DiveBombers>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Doldrums>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_DoubleEdge>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_EternalFlames>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_FerrisWheels>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Ifrit>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Inferno>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_MagmaOcean>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_MoulinRouge>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_OmegaTrinity>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Outbreak>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_PizzaCut>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Prometheus>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Prominence>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_SizzlingEdge>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Volcanics>(),10)
            .AddIngredient(ModContent.ItemType<Org13.Axel.Chacrams_Wildfire>(),10)

            .AddTile(TileID.LihzahrdFurnace)
            .AddTile(TileID.Furnaces)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }

    }
}
