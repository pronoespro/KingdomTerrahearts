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

namespace KingdomTerrahearts.Items.Weapons.Org13.Axel
{
    public class Chacrams_Corona : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corona");
            Tooltip.SetDefault("Flaming Oblivion" +
                "\nA weapon that possesses extreme Strength" +
                "\nDevastates tough enemies");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 67;
            Item.height = Item.width = 50;
            Item.knockBack = 7;
            Item.maxStack = 2;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 25;
            Item.useAnimation = 7;
            Item.useTime = 7;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Corona>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Corona>() };
            maxChakrams = 2;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_SizzlingEdge>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.CrimtaneBar, 5)
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>(), 3)
            .AddIngredient(ModContent.ItemType<Materials.pulsingStone>(), 3)
            .AddTile(TileID.LihzahrdFurnace)
            .Register();
        }

    }
}
