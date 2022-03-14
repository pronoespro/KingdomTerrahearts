using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.TreasureBags
{
    class DarksideTreasureBag:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }
        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            EntitySource_ItemOpen s = new EntitySource_ItemOpen(player,Type);

            player.TryGettingDevArmor(s);

            if (Main.rand.Next(100) < 3)
            {
                player.QuickSpawnItem(s,ModContent.ItemType<Weapons.Keyblade_demonite>());
            }
            else
            {
                player.QuickSpawnItem(s,ModContent.ItemType<Weapons.Keyblade_iron>());
            }

        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Darkside>();

    }
}
