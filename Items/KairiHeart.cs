using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class KairiHeart:ModItem
    {

        Mod fargoDLC;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kairi's heart");
            Tooltip.SetDefault("The heart of a princes of light");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.maxStack = 20;
            item.value = 10;
            item.rare = ItemRarityID.Blue;
            item.useAnimation = 40;
            item.useTime = 45;
            item.useStyle = ItemUseStyleID.HoldingUp;

        }

        public override bool CanUseItem(Player player)
        {
            fargoDLC = ModLoader.GetMod("FargowiltasSoulsDLC");
            return fargoDLC != null;
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, fargoDLC.NPCType("Echdeath"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

    }
}
