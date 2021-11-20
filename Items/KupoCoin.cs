using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class KupoCoin:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kupo Coin");
            Tooltip.SetDefault("A special coin that protects you from the worst" +
                "\nIf your HP drops to 0, it will restore half your health a single time" +
                "\nHas a three minutes cooldown");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.value = 40000;
            Item.rare = 1;
            Item.maxStack = 1;
        }

        public override void UpdateInventory(Player player)
        {
            int foundCoin = 0;
            for(int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type==Item.type && player.selectedItem!=i)
                {
                    if (foundCoin==0)
                    {
                        foundCoin++;
                    }
                    else
                    {
                        Item.NewItem(player.getRect(), ItemID.GoldCoin, 4);
                        player.ConsumeItem(ModContent.ItemType<KupoCoin>(),true);
                    }
                }
            }

        }

    }
}
