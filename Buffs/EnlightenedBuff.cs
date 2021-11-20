using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using KingdomTerrahearts.Items.Weapons;

namespace KingdomTerrahearts.Buffs
{
    class EnlightenedBuff:ModBuff
    {

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = "Light strenghtens you";
            base.ModifyBuffTip(ref tip, ref rare);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enlightened");
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            SoraPlayer sp= player.GetModPlayer<SoraPlayer>();
            sp.enlightened = true;
        }

    }
}
