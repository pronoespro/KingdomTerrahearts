using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class blazingStone : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blazing Stone");
            Tooltip.SetDefault("A splintered stone imbued with fire" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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

    public class betwixtStone : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Betwixt Stone");
            Tooltip.SetDefault("A splintered stone imbued with ambiguity" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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


    public class frostStone : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Stone");
            Tooltip.SetDefault("A splintered stone imbued with ice" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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


    public class lucidStone : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lucid Stone");
            Tooltip.SetDefault("A splintered stone imbued with light" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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



    public class mythrilStone : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril Stone");
            Tooltip.SetDefault("A splintered piece of mythril ore" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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


    public class pulsingStone : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pulsing Stone");
            Tooltip.SetDefault("A splintered stone imbued with might" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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



    public class lightningStone : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Stone");
            Tooltip.SetDefault("A splintered stone imbued with lightning" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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



    public class twilightStone : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight Stone");
            Tooltip.SetDefault("A splintered stone imbued with twilight" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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

    public class writhingStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Writhing Stone");
            Tooltip.SetDefault("A splintered stone imbued with darkness" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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
