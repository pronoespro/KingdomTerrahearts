using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class glide:AbilityBase
    {

        public float glideTime = 120;
        public bool noFallDamage = false;

        public override void SetStaticDefaults()
        {
            abilityName = "Glide";
            DisplayName.SetDefault(abilityName+" level " + (level + 1).ToString());
            Tooltip.SetDefault("A glide ability" +
                "\nAllows you to fall slowly");
        }

        public override void UpdateEquip(Player player)
        {
            if (noFallDamage)
                player.noFallDmg = true;
            abilityName = "Glide";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.canGlide = true;
            sp.glideTime += glideTime;
            sp.ChangeGlideFallSpeed(0.5f);
            base.UpdateEquip(player);
        }

        public override void UpdateInventory(Player player)
        {
            abilityName = "Glide";
            abilityTooltips = new string[]
            {
                "Lasts for "+ ((glideTime/60).ToString())+" seconds"
            };
            if (player == null)
            {
                SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                sp.canGlide = true;
                sp.glideTime += glideTime;
                sp.ChangeGlideFallSpeed(0.5f);
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
            sp.RaiseGlideLevel(this);
        }

        public override void ResetLevelEffects()
        {
            glideTime = 120;
            base.ResetLevelEffects();
        }

    }
}
