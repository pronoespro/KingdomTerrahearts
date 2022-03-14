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
    public class Chacrams_Outbreak : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Outbreak");
            Tooltip.SetDefault("Sunset Flames" +
                "\nA weapon that lets you string together longer combos");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 88;
            Item.height = Item.width = 50;
            Item.knockBack = 1;
            Item.maxStack = 12;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 30;
            Item.useAnimation = 6;
            Item.useTime = 6;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Outbreak>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Outbreak>() };
            maxChakrams = 12;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Outbreak>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(12)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.SilverBar, 5)
            .AddIngredient(ItemID.LeadBar, 5)
            .AddIngredient(ItemID.GoldBar)
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>(), 3)
            .AddIngredient(ModContent.ItemType<Materials.mythrilGem>())
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

    }
}
