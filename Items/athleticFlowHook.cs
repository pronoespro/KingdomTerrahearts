using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class athleticFlowHook :ModItem
    {
        int level=1;
        string hoolName = "Athletic Flow";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(hoolName+" level "+level.ToString());
            Tooltip.SetDefault("The ability to quicly dash to certain objects" +
                "\nUse as a grappling hook");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.scale = 0.1f;
            item.shootSpeed = 20;
            item.shoot = mod.ProjectileType("athleticFlowShotLock");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void UpdateInventory(Player player)
        {
            UpdateLevel();
        }



        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            UpdateLevel();
        }

        void UpdateLevel()
        {
            level = 1;
            if (NPC.downedBoss1)
                level++;
            if (NPC.downedBoss2)
                level++;
            if (NPC.downedBoss3)
                level++;
            if (NPC.downedSlimeKing)
                level++;
            if (NPC.downedQueenBee)
                level++;

            item.SetNameOverride(hoolName + " level " + level.ToString());
        }

    }
}
