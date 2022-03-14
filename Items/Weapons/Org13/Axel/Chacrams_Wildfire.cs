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
    public class Chacrams_Wildfire : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wildfire");
            Tooltip.SetDefault("Uncontrollable Pyre" +
                "\nA weapon that provides versatility by greatly boosting both Strength and Magic");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 107;
            Item.height = Item.width = 50;
            Item.knockBack = 5;
            Item.maxStack = 2;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 20;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Wildfire>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Wildfire>() };
            maxChakrams = 2;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Wildfire>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.LivingDemonFireBlock, 10)
            .AddIngredient(ItemID.LivingFireBlock, 10)
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>(), 3)
            .AddIngredient(ModContent.ItemType<Materials.pulsingStone>(), 3)
            .AddTile(TileID.LihzahrdFurnace)
            .Register();
        }

    }
}
