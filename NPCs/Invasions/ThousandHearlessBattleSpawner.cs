using System;
using KingdomTerrahearts.NPCs.Invasions;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs.Invasions
{
    class ThousandHearlessBattleSpawner:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Powerfull Heart");
            Tooltip.SetDefault("A heart so powerfull heartless need it" +
                "\nCalls a thousand heartless");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1;
            Item.maxStack = 99;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 3;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (!KingdomWorld.customInvasionUp)
                {
                    Main.NewText("A thousand heartless are approaching", 175, 75, 255);
                    ThousandHeartlessInvasion.StartInvasion();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return null;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.writhingShard>(), 5);
            recipe.Register();
        }

    }
}
