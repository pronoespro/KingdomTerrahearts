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
    public class Chacrams_PizzaCut : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pizza Cut");
            Tooltip.SetDefault("Buenitsima" +
                "\nThis looks awfully tasty");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 36;
            Item.height = Item.width = 50;
            Item.knockBack = 2;
            Item.maxStack = 2;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 20;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_PizzaCut>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_PizzaCut>() };
            maxChakrams = 2;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_PizzaCut>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.Pizza)
            .AddTile(TileID.LihzahrdFurnace)
            .Register();
        }

    }
}
