using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class Heal:AbilityBase
    {

        public int healAmmount = 30;
        public int invulnerability = 0;
        public int minManaCost = 40;

        public override void SetStaticDefaults()
        {
            abilityName = "Heal";

            DisplayName.SetDefault(abilityName + " level " + (level + 1).ToString());
            Tooltip.SetDefault("An ability that lets you use magic to heal" +
                "\nOnly activates if you already used a potion" +
                "\nWastes all MP");
        }

        public override void UpdateEquip(Player player)
        {
            abilityName = "Heal";
            abilityTooltips = new string[]
            {
                "Heals "+(int)((float)healAmmount / 100f * (float)player.statLifeMax)+" hp"
            };
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.canCastHeal = true;
            sp.castHealAmount=healAmmount;
            sp.castHealInvulnerabilityTime = invulnerability;
            sp.castHealCost = minManaCost;
            base.UpdateEquip(player);
        }

        public override void UpdateInventory(Player player)
        {
                abilityName = "Heal";
            if (player == null)
            {
                SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                sp.canCastHeal = true;
                sp.castHealAmount = healAmmount;
                sp.castHealInvulnerabilityTime = invulnerability;
                sp.castHealCost = minManaCost;
                base.UpdateInventory(player);
            }
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
            SoraPlayer sp = Main.player[item.owner].GetModPlayer<SoraPlayer>();
            sp.RaiseHealLevel(this);

        }

        public override void ResetLevelEffects()
        {
            base.ResetLevelEffects();
            healAmmount = 10;
            invulnerability = 0;
            minManaCost = 40;
        }

    }
}
