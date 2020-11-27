using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class MaxMobility :AbilityBase
    {


        float jumpHeight = 5;
        int jumpCount = 1;

        float glideTime = 120;
        bool noFallDamage = false;

        float dashSpeed = 7.5f;
        int dashReaload = 360;
        bool canDashMidair = false;

        public override void SetStaticDefaults()
        {
            abilityName = "Max Mobility";
            DisplayName.SetDefault(abilityName + " level " + (level + 1).ToString());
            Tooltip.SetDefault("An ability that combines all previous ones" +
                "\nAllows you to jump again in midair" +
                "\nAllows you to dash" +
                "\nAllows you to glide");
        }

        public override void UpdateEquip(Player player)
        {
            abilityName = "Max Mobility";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.canDoubleJump = true;
            sp.doubleJumpHeight += jumpHeight;
            sp.doubleJumpQuantity += jumpCount;

            if (noFallDamage)
                player.noFallDmg = true;
            sp.canGlide = true;
            sp.glideTime += glideTime;
            sp.ChangeGlideFallSpeed(0.5f);
            sp.dashSpeed += dashSpeed;
            sp.canDash = true;
            sp.canDashMidAir = canDashMidair;
            base.UpdateEquip(player);
        }

        public override void UpdateInventory(Player player)
        {
            initLvl = 3;
            abilityName = "Max Mobility";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.canDoubleJump = true;
            sp.doubleJumpHeight += jumpHeight;
            sp.doubleJumpQuantity += jumpCount;

            if (noFallDamage)
                player.noFallDmg = true;
            sp.canGlide = true;
            sp.glideTime += glideTime;
            sp.ChangeGlideFallSpeed(0.5f);

            sp.dashSpeed += dashSpeed;
            sp.canDash = true;
            sp.canDashMidAir = canDashMidair;
            sp.ChangeDashReload(dashReaload);
            base.UpdateInventory(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("doubleJump"));
            recipe.AddIngredient(mod.ItemType("glide"));
            recipe.AddIngredient(mod.ItemType("quickRun"));

            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }

        public override void RaiseLevel()
        {
            base.RaiseLevel();
            jumpHeight += 1.5f;
            if (level > 2)
            {
                jumpCount++;
            }

            glideTime += 60;
            if (level > 3)
            {
                noFallDamage = true;
            }

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
            glideTime = 120;
            
            jumpHeight = 7;
            jumpCount = 1;

            canDashMidair = false;
            dashSpeed = 7.5f;
        }

    }
}
