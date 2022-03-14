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
    public class Chacrams_BlazeOfGlory : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blaze of Glory");
            Tooltip.SetDefault("Axel's most basic weapon" +
                "\nA weapon that lets you string together faster, incredibly long combos");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 66;
            Item.height = Item.width = 50;
            Item.knockBack = 1;
            Item.maxStack = 5;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 20;
            Item.useAnimation = 7;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 7;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_BlazeOfGlory>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_BlazeOfGlory>() };
            maxChakrams = 5;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_BlazeOfGlory>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ModContent.ItemType<NPCs.Bosses.Org13.AxelSpawner>())
            .AddIngredient(ModContent.ItemType<Keyblade_flameFrolic>())
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>(),10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

    }
}
