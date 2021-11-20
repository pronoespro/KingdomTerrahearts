using Microsoft.Xna.Framework;
using System;
using Terraria;
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
            Item.rare = 9;
            Item.expert = true;
        }
        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();

            if (Main.rand.Next(100) < 3)
            {
                player.QuickSpawnItem(ModContent.ItemType<Weapons.Keyblade_demonite>());
            }
            else
            {
                player.QuickSpawnItem(ModContent.ItemType<Weapons.Keyblade_iron>());
            }

        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Darkside>();

    }
}
