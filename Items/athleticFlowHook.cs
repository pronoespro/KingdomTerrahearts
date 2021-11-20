using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
            Item.width = 10;
            Item.height = 10;
            Item.scale = 0.1f;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<Projectiles.athleticFlowShotLock>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
        public override void UpdateInventory(Player player)
        {
            UpdateLevel();

            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

            Item.shootSpeed = 10f;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity - player.velocity, type, damage, knockback);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            UpdateLevel();

            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

            Item.shootSpeed = 10f;
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

            Item.SetNameOverride(hoolName + " level " + level.ToString());
        }

    }
}
