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
    public class Chacrams_Ifrit : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ifrit");
            Tooltip.SetDefault("Axel's most basic weapon" +
                "\nA weapon that enables your attacks to reach a wide area and deal immense damage");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 57;
            Item.height = Item.width = 50;
            Item.knockBack = 1;
            Item.maxStack = 2;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 25;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Ifrit>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Ifrit>() };
            maxChakrams = 2;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Ifrit>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.UnholyTrident)
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>(), 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

    }
}
