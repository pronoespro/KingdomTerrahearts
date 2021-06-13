using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class seasaltIcecream:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seasalt Icecream");
            Tooltip.SetDefault("This ice cream's flavor mixes both a salty and a sweet taste");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 20;
            item.value = 10;
            item.rare = ItemRarityID.Blue;
            item.useAnimation = 40;
            item.useTime = 45;
            item.useStyle = ItemUseStyleID.HoldingUp;
            //item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool alreadySpawned = NPC.AnyNPCs(mod.NPCType("xion_firstPhase"));
            return !alreadySpawned;
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("xion_firstPhase"));
            Main.PlaySound(SoundID.FemaleHit, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("twilightShard"), 3);
            recipe.AddIngredient(ItemID.IceBlock,5);

            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }

    }
}
