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
    public class Chacrams_EternalFlames : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Flames");
            Tooltip.SetDefault("Axel's chakrams" +
                "\nHe tossed them away when he got a Keyblade");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 124;
            Item.height = Item.width=50;
            Item.knockBack = 8;
            Item.maxStack = 2;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 30;
            Item.useAnimation = 10;
            Item.UseSound =SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.value = 20000;
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chakram_EternalFlames>(), ModContent.ProjectileType<Projectiles.Weapons.Chakram_EternalFlames>() };
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chakram_EternalFlames>(), ModContent.ProjectileType<Projectiles.Weapons.Chakram_EternalFlames>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ItemID.LunarBar, 10)
            .AddIngredient(ItemID.FragmentSolar,10)
            .AddTile(TileID.Anvils)
            .Register();
        }

    }
}
