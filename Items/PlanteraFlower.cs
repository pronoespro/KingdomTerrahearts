using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class PlanteraFlower:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plantera Flower");
            Tooltip.SetDefault("A small plantera that calls to her mother" +
                "\nUse it to summon a new Plantera");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.maxStack = 20;
            item.rare = ItemRarityID.Blue;
            item.useAnimation = 40;
            item.useTime = 45;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool alreadySpawned = NPC.AnyNPCs(NPCID.Plantera);
            return !alreadySpawned && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, NPCID.Plantera);
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

    }
}
