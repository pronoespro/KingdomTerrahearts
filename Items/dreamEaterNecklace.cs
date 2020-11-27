using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace KingdomTerrahearts.Items
{
    public class dreamEaterNecklace:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dream Eater Necklace");
            Tooltip.SetDefault("A necklace that summons a rideable Dream Eater\n'MEW WOW' indeed");
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
            item.mountType = mod.MountType("mewwow");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar,3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
