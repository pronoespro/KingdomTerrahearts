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
    public class Chacrams_MoulinRouge : ChakramBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moulin Rouge");
            Tooltip.SetDefault("Dark Fire Chakrams" +
                "\nA weapon that lets you string together faster, much longer combos");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.damage = 55;
            Item.height = Item.width = 50;
            Item.knockBack = 3;
            Item.maxStack = 3;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Throwing;
            Item.rare = ItemRarityID.LightRed;
            Item.scale = 1;
            Item.shootSpeed = 25;
            Item.useAnimation = 7;
            Item.UseSound = SoundID.Item19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 7;
            Item.value = 20000;
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_MoulinRouge>(), ModContent.ProjectileType<Projectiles.Weapons.Chacrams_MoulinRouge>() };

            maxChakrams = 3;
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_MoulinRouge>(), ModContent.ProjectileType<Projectiles.Weapons.Chacrams_MoulinRouge>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>(),3)
            .AddIngredient(ItemID.SoulofNight,10)
            .AddIngredient(ModContent.ItemType<Materials.blazingStone>())
            .AddTile(TileID.Anvils)
            .Register();
        }

    }
}
