using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    class lastWorldDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Last World Dye");
            Tooltip.SetDefault("Leave your body and become a specter");
            GameShaders.Armor.BindShader(
                Item.type,
                new ArmorShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/lastWorldShader").Value), "ArmorLastWorldShader")
            );
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddTile(TileID.DyeVat);
            recipe.Register();
        }

    }
}
