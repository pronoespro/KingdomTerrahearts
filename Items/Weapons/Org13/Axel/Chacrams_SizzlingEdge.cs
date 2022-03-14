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
    public class Chacrams_SizzlingEdge : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sizzling Edge");
            Tooltip.SetDefault("Flaming Destruction" +
                "\nA weapon that possesses very high Strength" +
                "\nEffective against tough enemies");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 55;
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

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_SizzlingEdge>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_SizzlingEdge>() };
            maxChakrams = 2;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_SizzlingEdge>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.CrimtaneBar, 5)
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>(),3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

    }
}
