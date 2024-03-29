﻿using Microsoft.Xna.Framework;
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
    public class Chacrams_DelayedAction: ChakramBase
    { 

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Delayed Action");
            Tooltip.SetDefault("Demonite Chakrams" +
                "\nA weapon that shhots Dark Magic to give it a lot more power");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.damage = 36;
            Item.height = Item.width = 50;
            Item.knockBack = 3;
            Item.maxStack = 2;
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
            Item.value = 20000;
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_DelayedAction>(), ModContent.ProjectileType<Projectiles.Weapons.Chacrams_DelayedAction>() };
        }

        public override void UpdateInventory(Player player)
        {
            projectiles = new int[] { ModContent.ProjectileType<Projectiles.Weapons.Chacrams_DelayedAction>(), ModContent.ProjectileType<Projectiles.Weapons.Chacrams_DelayedAction>() };
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ItemID.DemoniteBar, 5)
            .AddIngredient(ItemID.ShadowScale, 5)
            .AddIngredient(ModContent.ItemType<Chacrams_Ashes>())
            .AddTile(TileID.Anvils)
            .Register();
        }

    }
}
