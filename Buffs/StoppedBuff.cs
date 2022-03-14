using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace KingdomTerrahearts.Buffs
{
    public class StoppedBuff:ModBuff
    {
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = "You are stuck in time";
            base.ModifyBuffTip(ref tip, ref rare);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enlightened");
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            SoraPlayer sp = Main.LocalPlayer.GetModPlayer<SoraPlayer>();

            if (sp.isBoss(npc.whoAmI))
            {
                npc.life -= 1;
                npc.checkDead();
            }
            else
            {
                NPCOverride ov = npc.GetGlobalNPC<NPCOverride>();

                ov.Stop(npc);
                npc.velocity = Vector2.Zero;
                npc.color = Color.Gray;
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.noControlTime = 2;
            player.velocity = Vector2.Zero;
            player.gravity = 0;
        }

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            return true;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            return true;
        }

    }
}
