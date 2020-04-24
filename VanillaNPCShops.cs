using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts
{
    class VanillaNPCShops:GlobalNPC
    {

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Dryad:
                    shop.item[nextSlot].SetDefaults(mod.ItemType("Keyblade_destiny"));
                    nextSlot++;
                    break;
                case NPCID.WitchDoctor:

                    shop.item[nextSlot].SetDefaults(mod.ItemType("Keyblade_witchDoctor"));
                    nextSlot++;
                    break;
                case NPCID.Merchant:
                    shop.item[nextSlot].SetDefaults(mod.ItemType("KupoCoin"));
                    nextSlot++;
                    break;
            }
        }

        public override void NPCLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.QueenBee:
                    Item.NewItem(npc.frame, ItemID.HoneyBalloon);
                    break;
            }
        }

    }
}
