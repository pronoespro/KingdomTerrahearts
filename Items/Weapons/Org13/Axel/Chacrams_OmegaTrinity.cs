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
    public class Chacrams_OmegaTrinity : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omega Trinity");
            Tooltip.SetDefault("Eternal Inferno" +
                "\nA weapon that lets you string together longer combos with wider reach");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 84;
            Item.height = Item.width = 50;
            Item.knockBack = 5;
            Item.maxStack = 6;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 35;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_OmegaTrinity>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_OmegaTrinity>() };
            maxChakrams = 6;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_OmegaTrinity>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(6)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.GoldBar, 2)
            .AddIngredient(ItemID.CrimtaneBar, 2)
            .AddIngredient(ItemID.LeadBar, 5)
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>(), 3)
            .AddIngredient(ModContent.ItemType<Materials.mythrilStone>())
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

    }
}
