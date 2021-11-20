using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class MaxMobility :AbilityBase
    {


        public float jumpHeight = 10;
        public int jumpCount = 1;

        public float glideTime = 120;
        public bool noFallDamage = false;

        public float dashSpeed = 7.5f;
        public int dashReaload = 360;
        public bool canDashMidair = false;

        public override void SetStaticDefaults()
        {
            abilityName = "Max Mobility";
            DisplayName.SetDefault(abilityName + " level " + (level + 1).ToString());
            Tooltip.SetDefault("An ability that combines all previous mobility ones" +
                "\nAllows you to jump again in midair" +
                "\nAllows you to dash" +
                "\nAllows you to glide");
        }

        public override void UpdateEquip(Player player)
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
            base.UpdateEquip(player);
        }

        public override void UpdateInventory(Player player)
        {
            abilityName = "Max Mobility";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            initLvl = 3;
            if (player == null)
            {
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

            abilityTooltips = new string[]
            {
                "Glide lasts for "+ ((glideTime/60).ToString())+" seconds" +
                "\n"+((canDashMidair)?"Dash can be used in mid-air":"Dash cannot be used midair")
            };
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<doubleJump>())
            .AddIngredient(ModContent.ItemType<glide>())
            .AddIngredient(ModContent.ItemType<quickRun>())
            .AddTile(TileID.TinkerersWorkbench)
            .Register();

        }

        public override void RaiseLevel()
        {
            base.RaiseLevel();
            SoraPlayer sp = Main.player[Item.playerIndexTheItemIsReservedFor].GetModPlayer<SoraPlayer>();
            sp.RaiseMobilityLevel(this);
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
