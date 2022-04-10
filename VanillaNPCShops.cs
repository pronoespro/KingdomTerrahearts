using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace KingdomTerrahearts
{
    class VanillaNPCShops:GlobalNPC
    {

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Dryad:
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Keyblade_destiny>());
                    nextSlot++;
                    break;
                case NPCID.WitchDoctor:

                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Keyblade_witchDoctor>());
                    nextSlot++;
                    break;
                case NPCID.Merchant:
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.KupoCoin>());
                    nextSlot++;
                    break;
            }
        }

    }
}
