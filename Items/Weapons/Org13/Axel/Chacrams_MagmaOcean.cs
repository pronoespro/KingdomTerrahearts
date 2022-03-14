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
    public class Chacrams_MagmaOcean : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Ocean");
            Tooltip.SetDefault("Air Judgement" +
                "\nA weapon that performs very well in midair" +
                "\nExcellent for taking on fliers");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.damage = 44;
            Item.height = Item.width = 50;
            Item.knockBack = 1;
            Item.maxStack = 1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 25;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_MagmaOcean>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_MagmaOcean>() };
            maxChakrams = 1;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_MagmaOcean>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.Shuriken,10)
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>(), 1)
            .AddTile(TileID.Anvils)
            .Register();
        }

    }
}
