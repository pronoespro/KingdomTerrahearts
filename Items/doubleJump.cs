using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    class doubleJump : AbilityBase
    {

        float jumpHeight = 5;
        int jumpCount = 1;

        public override void SetStaticDefaults()
        {
            abilityName = "Double Jump";
            DisplayName.SetDefault(abilityName+" level " + (level+1).ToString());
            Tooltip.SetDefault("A double jump ability" +
                "\nAllows you to jump again in midair" +
                "\nVery low jump height");
        }

        public override void UpdateEquip(Player player)
        {
            abilityName = "Double Jump";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.canDoubleJump = true;
            sp.doubleJumpHeight += jumpHeight;
            sp.doubleJumpQuantity+=jumpCount;
            base.UpdateEquip(player);
        }

        public override void UpdateInventory(Player player)
        {
            abilityName = "Double Jump";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.canDoubleJump = true;
            sp.doubleJumpHeight += jumpHeight;
            sp.doubleJumpQuantity += jumpCount;
            base.UpdateInventory(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.FallenStar, 2);

            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }

        public override void RaiseLevel()
        {
            jumpHeight += 1.5f;
            if (level > 2)
            {
                jumpCount++;
            }
            base.RaiseLevel();
        }

        public override void ResetLevelEffects()
        {
            base.ResetLevelEffects();
            jumpHeight = 5;
            jumpCount = 1;
        }
    }
}
