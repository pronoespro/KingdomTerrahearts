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
    public class Chacrams_DoubleEdge : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Double Edge");
            Tooltip.SetDefault("Dual Fire" +
                "\nA weapon that provides versatility by boosting both Strength and Magic");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 72;
            Item.height = Item.width = 50;
            Item.knockBack = 3;
            Item.maxStack = 2;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 20;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_DoubleEdge>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_DoubleEdge>() };
            maxChakrams = 2;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_DoubleEdge>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.CobaltBar, 5)
            .AddIngredient(ItemID.DemoniteBar, 5)
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>(), 3)
            .AddIngredient(ModContent.ItemType<Materials.betwixtStone>(), 3)
            .AddIngredient(ModContent.ItemType<Materials.pulsingStone>(), 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

    }
}
