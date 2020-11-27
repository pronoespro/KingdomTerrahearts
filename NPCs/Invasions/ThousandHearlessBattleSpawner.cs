using System;
using KingdomTerrahearts.NPCs.Invasions;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs.Invasions
{
    class ThousandHearlessBattleSpawner:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Powerfull Heart");
            Tooltip.SetDefault("A heart so powerfull heartless need it." +
                "\nCalls a thousand heartless.");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.scale = 1;
            item.maxStack = 99;
            item.useTime = 30;
            item.useAnimation = 30;
            item.UseSound = SoundID.Item1;
            item.useStyle = 1;
            item.consumable = true;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = 3;
        }

        public override bool UseItem(Player player)
        {
            if (!KingdomWorld.customInvasionUp)
            {
                Main.NewText("A thousand heartless are approaching.", 175, 75, 255, false);
                ThousandHeartlessInvasion.StartInvasion();
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("lucidShard"), 5);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
