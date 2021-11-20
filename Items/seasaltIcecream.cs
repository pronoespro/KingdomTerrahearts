using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;

namespace KingdomTerrahearts.Items
{
    public class seasaltIcecream:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seasalt Icecream");
            Tooltip.SetDefault("This ice cream's flavor mixes both a salty and a sweet taste");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 40;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            //Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Org13.xion_firstPhase>())|| NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Org13.xion_secondPhase>()) || NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Org13.xion_finalPhase>());
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Bosses.Org13.xion_firstPhase>());
            SoundEngine.PlaySound(SoundID.FemaleHit, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(), 3)
            .AddIngredient(ItemID.IceBlock,5)
            .AddTile(TileID.WorkBenches)
            .Register();

        }

    }
}
