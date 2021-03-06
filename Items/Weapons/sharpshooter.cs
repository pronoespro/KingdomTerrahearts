﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    class sharpshooter:ModItem
    {

        int combo=0;
        int comboMax = 32;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arrowgun");
            Tooltip.SetDefault("A weapon that shoots out dark arrows" +
                "\nConsumes 1 mana per arrow" +
                "\nHas combos" +
                "\nCan shoot up to 32 shots before needing to reload");
        }

        public override void SetDefaults()
        {
            item.damage = 17*2;
            item.magic = true;
            item.mana = 3;
            item.width = 50;
            item.height = 50;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.knockBack = 3;
            item.value = 10000;
            item.rare = 2;
            item.UseSound = SoundID.Item5.WithVolume(0.25f).WithPitchVariance(0.5f);
            item.autoReuse = true;
            item.shoot = ProjectileID.JestersArrow;
            item.shootSpeed = 25;
            item.noMelee = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                combo = 0;
                return true;
            }
            else
            {
                return base.UseItem(player);
            }
        }

        public override bool CanUseItem(Player player)
        {

            combo++;
            if (combo >= comboMax)
            {
                item.reuseDelay = 200;
                combo = 0;
            }
            else
            {
                item.reuseDelay = 0;
            }

            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FlareGun);
            recipe.AddIngredient(ItemID.Handgun);
            recipe.AddIngredient(ItemID.Flare, 13);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
