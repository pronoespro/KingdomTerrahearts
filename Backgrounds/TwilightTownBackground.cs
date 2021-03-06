﻿using Terraria;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Backgrounds
{

    public class TwilightTownBackground:ModSurfaceBgStyle
    {

        public override bool ChooseBgStyle()
        {
            return !Main.gameMenu && Main.player[Main.myPlayer].GetModPlayer<SoraPlayer>().inTwilightTown;
        }

        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for(int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    fades[i] = (fades[i] > 1) ? 1 : fades[i];
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    fades[i] = (fades[i] <0) ? 0 : fades[i];
                }
            }
        }

        public override int ChooseFarTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/Background_TwilightTown5");
        }

        public override int ChooseMiddleTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/Background_TwilightTown3");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            scale = 1.25f;
            return mod.GetBackgroundSlot("Backgrounds/Background_TwilightTown");
        }

    }
}
