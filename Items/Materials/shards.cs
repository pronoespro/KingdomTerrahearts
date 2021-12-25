using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Materials
{
    public class blazingShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blazing Shard");
            Tooltip.SetDefault("A sliver of magic that is warm to the touch" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
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

    public class betwixtShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Betwixt Shard");
            Tooltip.SetDefault("A sliver of magic with an energy neither fully light nor dark" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
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


    public class frostShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Shard");
            Tooltip.SetDefault("A sliver of magic that is cold to the touch" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
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


    public class lucidShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lucid Shard");
            Tooltip.SetDefault("A sliver of magic that shimmers with promise" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
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



    public class mythrilShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mythril Shard");
            Tooltip.SetDefault("A sliver of mythril ore" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
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


    public class pulsingShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pulsing Shard");
            Tooltip.SetDefault("A sliver of magic that rumbles with strength" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
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



    public class lightningShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Shard");
            Tooltip.SetDefault("A sliver of magic that tingles when held" +
                "\nUsed for Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
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



    public class twilightShard : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight Shard");
            Tooltip.SetDefault("A sliver of magic that glows like the setting sun" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
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

    public class writhingShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Writhing Shard");
            Tooltip.SetDefault("A sliver of magic that stays bathed in shadow" +
                "\nA very common ingredient of Item synthesis");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
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
