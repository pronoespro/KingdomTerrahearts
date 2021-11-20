using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Buffs
{
    public class mewwowBuff:ModBuff
    {

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = "You are mounting a Dream Eater";
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mew Wow");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.mewwow>(),player);
            player.buffTime[buffIndex] = 10;
        }

    }
}
