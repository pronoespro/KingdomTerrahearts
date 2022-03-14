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
    public class Chacrams_Burnout : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burnout");
            Tooltip.SetDefault("Magical Burn" +
                "\nA weapon that boasts superior Magic and combo speed");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 73;
            Item.height = Item.width = 50;
            Item.knockBack = 5;
            Item.maxStack = 4;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 20;
            Item.useAnimation = 7;
            Item.useTime = 7;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 2000;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Burnout>();
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Burnout>() };
            maxChakrams = 4;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_Burnout>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddIngredient(ItemID.DemoniteBar, 10)
            .AddIngredient(ItemID.ShadowScale, 5)
            .AddIngredient(ModContent.ItemType<Materials.pulsingStone>(), 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }

    }
}
