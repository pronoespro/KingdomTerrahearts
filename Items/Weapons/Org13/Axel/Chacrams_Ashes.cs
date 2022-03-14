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
    public class Chacrams_Ashes : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashes");
            Tooltip.SetDefault("Axel's most basic weapon" +
                "\nNothing really sets it apart");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.damage = 15;
            Item.height = Item.width = 50;
            Item.knockBack = 1;
            Item.maxStack = 1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 15;
            Item.useAnimation = 15;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Ashes>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Ashes>()};
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Ashes>()};
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.WoodenBoomerang)
            .AddIngredient(ItemID.IronBar)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe()
            .AddIngredient(ItemID.WoodenBoomerang)
            .AddIngredient(ItemID.LeadBar)
            .AddTile(TileID.Anvils)
            .Register();
        }

    }
}
