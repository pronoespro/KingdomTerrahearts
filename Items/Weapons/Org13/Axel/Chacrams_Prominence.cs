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
    public class Chacrams_Prominence : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prominence");
            Tooltip.SetDefault("Pyromancy's Power" +
                "\nA formidable weapon with exceptional capabilities");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 92;
            Item.height = Item.width = 50;
            Item.knockBack = 3;
            Item.maxStack = 1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 35;
            Item.useAnimation = 5;
            Item.useTime = 5;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Prominence>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Prominence>() };
            maxChakrams = 1;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_SizzlingEdge>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.GoldBar, 20)
            .AddIngredient(ItemID.PalladiumBar, 20)
            .AddIngredient(ItemID.Ruby, 5)
            .AddIngredient(ModContent.ItemType<Materials.blazingGem>())
            .AddIngredient(ModContent.ItemType<Materials.lucidGem>())
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

    }
}
