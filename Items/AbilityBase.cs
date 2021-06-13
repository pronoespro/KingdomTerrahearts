using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public abstract class AbilityBase : ModItem
    {

        public int level = 0;
        public string abilityName;
        int tooltipStart = 4;
        public string[] abilityTooltips = new string[]
        {
            "this is a tooltip",
            "Tooltip level 2"
        };
        public int initLvl = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Missing ability");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {

            item.accessory = true;
            item.width = 10;
            item.height = 10;
            item.scale = 0.1f;

        }

        public override void UpdateInventory(Player player)
        {
            CheckLevel();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CheckLevel();
        }

        public virtual void CheckLevel()
        {
            level = initLvl;
            ResetLevelEffects();
            SoraPlayer sp = Main.player[item.owner].GetModPlayer<SoraPlayer>();
            for (int i = 0; i < sp.CheckPlayerLevel(); i++)
            {
                RaiseLevel();
            }
        }

        public virtual void ResetLevelEffects()
        {
            ChangeNameByLevel();
        }

        public virtual void RaiseLevel()
        {
            level++;
            ChangeNameByLevel();
        }
        
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            int lvl = (level >= abilityTooltips.Length) ? abilityTooltips.Length-1 : level;
            TooltipLine line = new TooltipLine(mod, "level tooltip", abilityTooltips[lvl]);

            tooltips.Add(line);
        }

        public virtual void ChangeNameByLevel()
        {
            item.SetNameOverride(abilityName + " level " + (level+1));
        }

    }
}
