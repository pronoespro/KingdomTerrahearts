using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class blazingGem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blazing Gem");
            Tooltip.SetDefault("A fine jewel that can barely contain the blaze within it" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }

    }

    public class betwixtGem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Betwixt Gem");
            Tooltip.SetDefault("A fine jewel that can barely contain the uncertainty within it" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }
    }


    public class frostGem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Gem");
            Tooltip.SetDefault("A fine jewel that can barely contain the cold within it" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }
    }


    public class lucidGem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lucid Gem");
            Tooltip.SetDefault("A fine jewel that can barely contain the radiance within it" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }

    }



    public class mythrilGem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril Gem");
            Tooltip.SetDefault("A fine jewel of refined mythril" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }
    }


    public class pulsingGem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pulsing Gem");
            Tooltip.SetDefault("A fine jewel that can barely contain the force within it" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }
    }



    public class lightningGem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Gem");
            Tooltip.SetDefault("A fine jewel that can barely contain the electricity within it" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }
    }



    public class twilightGem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight Gem");
            Tooltip.SetDefault("A fine jewel that can barely contain the twilight within it" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }
    }

    public class writhingGem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Writhing Gem");
            Tooltip.SetDefault("A fine jewel that can barely contain the blackness within it" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.maxStack = 999;
        }
    }

}
