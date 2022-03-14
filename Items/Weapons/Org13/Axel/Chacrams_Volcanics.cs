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
    public class Chacrams_Volcanics : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Volcanics");
            Tooltip.SetDefault("Aerial End" +
                "\nA weapon that performs extremely well in midair" +
                "\nOutstanding for taking on fliers");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 62;
            Item.height = Item.width = 50;
            Item.knockBack = 1;
            Item.maxStack = 2;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 40;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Volcanics>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Volcanics>() };
            maxChakrams = 2;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Volcanics>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.PalladiumBar, 5)
            .AddIngredient(ItemID.LeadBar, 5)
            .AddIngredient(ModContent.ItemType<Materials.blazingShard>(), 10)
            .AddIngredient(ModContent.ItemType<Materials.lucidShard>(), 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

    }
}
