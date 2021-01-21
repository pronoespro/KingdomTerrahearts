using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KingdomTerrahearts.Buffs
{
    class PMemberBuff:ModBuff
    {

        public override void SetDefaults()
        {
            DisplayName.SetDefault("PartyMember");
            Description.SetDefault("Now you work for the player");
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = "You now work for "+Main.player[Main.myPlayer].name;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.color = Color.LightBlue;
            npc.AddBuff(mod.BuffType("PMemberBuff"), 1000000000);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.ClearBuff(Type);
            buffIndex--;
        }

    }
}
