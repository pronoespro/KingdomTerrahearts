using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    class slidingDash : HabilityBase
    {

        float dashSpeed = 7.5f;
        int dashReaload = 360;
        bool canDashMidair = false;
        
        public override void SetStaticDefaults()
        {
            habilityName = "Sliding Dash";
            DisplayName.SetDefault(habilityName+" level " + (level+1).ToString());
            Tooltip.SetDefault("A sliding dash ability" +
                "\nAllows you to dash and avoid attacks" +
                "\nVery low speed" +
                "\nCan only be used on the ground");
        }

        public override void UpdateEquip(Player player)
        {
            habilityName = "Sliding Dash";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.dashSpeed += dashSpeed;
            sp.canDash = true;
            base.UpdateEquip(player);
        }

        public override void UpdateInventory(Player player)
        {
            habilityName = "Sliding Dash";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.dashSpeed += dashSpeed;
            sp.canDash = true;
            sp.canDashMidAir = canDashMidair;
            sp.ChangeDashReload(dashReaload);
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
            base.RaiseLevel();
            dashSpeed += 2.5f;
            dashReaload -= 30;
            if (dashReaload <= 30)
            {
                dashReaload = 30;
            }
            if (level > 2)
            {
                canDashMidair = true;
            }
        }

        public override void ResetLevelEffects()
        {
            base.ResetLevelEffects();
            canDashMidair = false;
            dashSpeed = 7.5f;
        }
    }
}
