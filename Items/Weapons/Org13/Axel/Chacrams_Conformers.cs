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
    public class Chacrams_Conformers : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Conformers");
            Tooltip.SetDefault("Axel's Fiery Might" +
                "\nA weapon that draws forth its true wielder's personality");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 90;
            Item.height = Item.width = 50;
            Item.knockBack = 1;
            Item.maxStack = 20;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 20;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation =6;
            Item.useTime = 6;
            Item.value = 20000;

            maxChakrams = 20;
            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Conformers>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Conformers>() };
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Conformers>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(20)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.DemoniteBar,10)
            .AddIngredient(ItemID.CrimtaneBar,10)
            .AddIngredient(ItemID.FragmentSolar,10)
            .AddIngredient(ItemID.LunarBar,2)
            .AddIngredient(ModContent.ItemType<Materials.blazingCrystal>())
            .AddIngredient(ModContent.ItemType<Materials.twilightCrystal>())
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }

    }
}
