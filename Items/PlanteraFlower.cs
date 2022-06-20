using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;

namespace KingdomTerrahearts.Items
{
    public class PlanteraFlower:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plantera Flower");
            Tooltip.SetDefault("A small plantera that calls to her mother" +
                "\nUse it to summon a new Plantera");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 40;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool alreadySpawned = NPC.AnyNPCs(NPCID.Plantera);
            return !alreadySpawned && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
        }

        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            NPC.SpawnOnPlayer(player.whoAmI, NPCID.Plantera);
            SoundEngine.PlaySound(SoundID.Roar, player.position);
            return true;
        }

    }
}
