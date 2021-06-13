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
    public class Chacrams_EternalFlames : BaseChakram
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Flames");
            Tooltip.SetDefault("Axel's chakrams" +
                "\nHe tossed them away when he got a Keyblade");
        }

        public override void SetDefaults()
        {
            item.autoReuse = true;
            item.damage = 50;
            item.height = item.width=50;
            item.knockBack = 8;
            item.maxStack = 2;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.melee = true;
            item.rare = ItemRarityID.LightRed;
            item.scale = 1;
            item.shootSpeed = 15;
            item.useAnimation = 15;
            item.UseSound =SoundID.Item19;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 15;
            item.value = 20000;
            projectiles = new int[] { mod.ProjectileType("Chakram_EternalFlames"), mod.ProjectileType("Chakram_EternalFlames") };
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { mod.ProjectileType("Chakram_EternalFlames"), mod.ProjectileType("Chakram_EternalFlames") };
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddIngredient(ItemID.Flamarang);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this,2);
            recipe.AddRecipe();
        }

    }
}
