using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class quickRun : AbilityBase
    {

        public float dashSpeed = 10;
        public int dashReaload = 360;
        public bool canDashMidair = false;
        
        public override void SetStaticDefaults()
        {
            abilityName = "Quick Run";
            DisplayName.SetDefault(abilityName+" level " + (level+1).ToString());
            Tooltip.SetDefault("An ability that gives you a quick dash" +
                "\nAllows you to avoid attacks, no invulnerability" +
                "\nVery low speed" +
                "\nCan only be used on the ground");
        }

        public override void UpdateEquip(Player player)
        {
            abilityName = "Quick Run";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.dashSpeed += dashSpeed;
            sp.canDash = true;
            sp.canDashMidAir = canDashMidair;
            sp.ChangeDashReload(dashReaload);
            abilityTooltips = new string[]
            {
                "Lets you dash "+(level>3?"a short distance": ((level>7)?"a long distance":"some distance"))+(canDashMidair?"and you can dash mid-air.":"but only on the ground.")
            };
        }

        public override void UpdateInventory(Player player)
        {
                abilityName = "Quick Run";
            if (player == null)
            {
                SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                sp.dashSpeed += dashSpeed;
                sp.canDash = true;
                sp.canDashMidAir = canDashMidair;
                sp.ChangeDashReload(dashReaload);
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
            sp.RaiseQuickRunLevel(this);
        }

        public override void ResetLevelEffects()
        {
            base.ResetLevelEffects();
            canDashMidair = false;
            dashSpeed = 7.5f;
        }
    }
}
