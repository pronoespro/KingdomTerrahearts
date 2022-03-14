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
    public class Chacrams_Combustion : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Combustion");
            Tooltip.SetDefault("Flaming flames of firy fire" +
                "\nA weapon that lets you string together faster combos");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 27;
            Item.height = Item.width = 50;
            Item.knockBack = 1;
            Item.maxStack = 1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 15;
            Item.useAnimation = 5;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 5;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Combustion>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Combustion>() };
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Combustion>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Flamarang)
            .AddIngredient(ItemID.HellstoneBar,2)
            .AddIngredient(ModContent.ItemType<Materials.blazingShard>(),10)
            .AddTile(TileID.Anvils)
            .Register();
        }

    }
}
