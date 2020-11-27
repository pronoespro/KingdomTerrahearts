using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using KingdomTerrahearts.Items.Weapons;

namespace KingdomTerrahearts
{
    class ItemOverride : GlobalItem
    {

        public override bool UseItem(Item item, Player player)
        {

            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

            switch (item.type)
            {
                case ItemID.FallenStar:

                    player.AddBuff(mod.BuffType("EnlightenedBuff"),30*60);

                    break;
            }

            return base.UseItem(item, player);
        }

        public override bool CanUseItem(Item item, Player player)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            switch (item.type)
            {
                case ItemID.WormFood:
                    if (sp.fightingInBattleground)
                        return true;
                    break;
            }

            return base.CanUseItem(item, player);
        }

    }
}
