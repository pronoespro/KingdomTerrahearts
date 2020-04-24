using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    class secondChance:AbilityBase
    {

        int invulnerabilityFrames = 1;
        int reloadTime = 180;
        bool autoHP = false;
        int autoHPReload = 180*60;
        int recoveredHp = 1;

        public override void SetStaticDefaults()
        {
            abilityName = "Second Chance";

            abilityTooltips = new string[] {
                "An ability that lets you keep one hp after a fatal attack \nWorks every three minutes.",
                "An ability that lets you keep one hp after a fatal attack \nWorks every three minutes.",
                "An ability that lets you keep one hp after a fatal attack \nWorks every two minutes and a half.",
                "An ability that lets you keep one hp after a fatal attack \nWorks every two minutes."

            };

            DisplayName.SetDefault(abilityName + " level " + (level + 1).ToString());
        }

        public override void UpdateEquip(Player player)
        {
            abilityName = "Second Chance";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.hasSecondChance = true;
            sp.secondChanceInvulnerability = invulnerabilityFrames;
            sp.secondChanceReload = reloadTime;
            sp.hasAutoHP = autoHP;
            sp.autoHPRecover = recoveredHp;
            sp.autoHPReload = autoHPReload;
            base.UpdateEquip(player);
        }

        public override void UpdateInventory(Player player)
        {
            abilityName = "Second Chance";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.hasSecondChance = true;
            sp.secondChanceInvulnerability = invulnerabilityFrames;
            sp.secondChanceReload = reloadTime;
            sp.hasAutoHP = autoHP;
            sp.autoHPRecover = recoveredHp;
            sp.autoHPReload = autoHPReload;
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
            invulnerabilityFrames += 2;
            reloadTime -= 10;
            if (level > 5)
            {
                autoHP = true;
                autoHPReload -= 60*15;
                recoveredHp += 10;
            }

        }

        public override void ResetLevelEffects()
        {
            base.ResetLevelEffects();
            invulnerabilityFrames = 1;
            recoveredHp = 1;
            reloadTime = 180;
            autoHP = false;
            autoHPReload = 180 * 60;
            recoveredHp = 1;
        }

    }
}
