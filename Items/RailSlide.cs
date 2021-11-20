using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace KingdomTerrahearts.Items
{
    public class RailSlide:ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rail Slide Ability");
            Tooltip.SetDefault("An ability that allows you to use the Rail Slide ability" +
                "\nFly through the sky");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 100;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item79;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<Mounts.FlowmotionPath>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FallenStar, 3)
            .AddTile(TileID.WorkBenches)
            .Register();
        }

    }
}
