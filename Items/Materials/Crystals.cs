using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class blazingCrystal : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blazing crystal");
            Tooltip.SetDefault("A miraculous crystal formed of pure fire" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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

    public class betwixtCrystal : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Betwixt Crystal");
            Tooltip.SetDefault("A miraculous crystal formed of pure ambiguity" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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


    public class frostCrystal : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Crystal");
            Tooltip.SetDefault("A miraculous crystal formed of pure cold" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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


    public class lucidCrystal : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lucid Crystal");
            Tooltip.SetDefault("A miraculous crystal formed of pure light" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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



    public class mythrilCrystal : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril Crystal");
            Tooltip.SetDefault("A miraculous crystal formed of pure mythril" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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


    public class pulsingCrystal : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pulsing Crystal");
            Tooltip.SetDefault("A miraculous crystal formed of pure might" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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



    public class lightningCrystal : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Crystal");
            Tooltip.SetDefault("A miraculous crystal formed of pure lightning" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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



    public class twilightCrystal : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight Crystal");
            Tooltip.SetDefault("A miraculous crystal formed of pure twilight" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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

    public class writhingCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Writhing Crystal");
            Tooltip.SetDefault("A miraculous crystal formed of pure darkness" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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
