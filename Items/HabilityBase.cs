using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public abstract class HabilityBase : ModItem
    {

        public int level = 0;
        public string habilityName;

        public override void SetDefaults()
        {

            item.accessory = true;
            item.width = 10;
            item.height = 10;

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
            level = 0;
            ResetLevelEffects();
            if (NPC.downedBoss1)
                RaiseLevel();
            if (NPC.downedBoss2)
                RaiseLevel();
            if (NPC.downedBoss3)
                RaiseLevel();
            if (NPC.downedSlimeKing)
                RaiseLevel();
            if (NPC.downedQueenBee)
                RaiseLevel();
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

        public virtual void ChangeNameByLevel()
        {
            item.SetNameOverride(habilityName + " level " + (level+1));
        }

    }
}
