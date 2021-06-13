using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace KingdomTerrahearts.Items
{
    public class RailSlide:ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rail Slide Ability");
            Tooltip.SetDefault("An ability that allows you to use the Rail Slide ability" +
                "\nFly through the sky");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = 100;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.mountType = mod.MountType("FlowmotionPath");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
