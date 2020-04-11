using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    class glide:HabilityBase
    {

        float glideTime = 120;

        public override void SetStaticDefaults()
        {
            habilityName = "Glide";
            DisplayName.SetDefault(habilityName+" level " + (level + 1).ToString());
            Tooltip.SetDefault("A glide ability" +
                "\nAllows you to fall slowly" +
                "\nOnly lasts for 2 seconds");
        }

        public override void UpdateEquip(Player player)
        {
            habilityName = "Glide";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.canGlide = true;
            sp.glideTime += glideTime;
            sp.ChangeGlideFallSpeed(0.5f);
            base.UpdateEquip(player);
        }

        public override void UpdateInventory(Player player)
        {
            habilityName = "Glide";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.canGlide = true;
            sp.glideTime += glideTime;
            sp.ChangeGlideFallSpeed(0.5f);
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
            glideTime += 60;
            base.RaiseLevel();
        }

        public override void ResetLevelEffects()
        {
            glideTime = 120;
            base.ResetLevelEffects();
        }

    }
}
