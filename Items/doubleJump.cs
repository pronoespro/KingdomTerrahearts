using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class doubleJump : AbilityBase
    {

        public float jumpHeight = 10;
        public int jumpCount = 1;

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
            if (player==null)
            {
                abilityName = "Double Jump";
                SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                sp.canDoubleJump = true;
                sp.doubleJumpHeight += jumpHeight;
                sp.doubleJumpQuantity += jumpCount;
            }
            base.UpdateInventory(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FallenStar, 2)
            .AddTile(TileID.WorkBenches)
            .Register();

        }

        public override void RaiseLevel()
        {
            base.RaiseLevel();
            SoraPlayer sp = Main.player[Item.playerIndexTheItemIsReservedFor].GetModPlayer<SoraPlayer>();
            sp.RaiseDoubleJumpLevel(this);
        }

        public override void ResetLevelEffects()
        {
            base.ResetLevelEffects();
            jumpHeight = 5;
            jumpCount = 1;
        }
    }
}
