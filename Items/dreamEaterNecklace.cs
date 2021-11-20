using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace KingdomTerrahearts.Items
{
    public class dreamEaterNecklace:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dream Eater Necklace");
            Tooltip.SetDefault("A necklace that summons a rideable Dream Eater\n'MEW WOW' indeed");
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
            Item.mountType = ModContent.MountType<Mounts.mewwow>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FallenStar,3)
            .AddTile(TileID.WorkBenches)
            .Register();
        }

    }
}
