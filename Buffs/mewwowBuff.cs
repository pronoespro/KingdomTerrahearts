using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Buffs
{
    public class mewwowBuff:ModBuff
    {

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Mew Wow");
            Description.SetDefault("You are mounting a Dream Eater");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("mewwow"),player);
            player.buffTime[buffIndex] = 10;
        }

    }
}
