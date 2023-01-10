using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace KingdomTerrahearts.Backgrounds
{

	public class TwilightTownBackground : ModSurfaceBackgroundStyle
	{

		public static bool[] TwilightBackgroundStyleSwap;

		public override void Load()
		{
			base.Load();
			TwilightBackgroundStyleSwap = new bool[2];
		}

		public override void Unload()
		{
			TwilightBackgroundStyleSwap = null;
		}

		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++)
			{
				if (i == Slot)
				{
					fades[i] += transitionSpeed;
					fades[i] = (fades[i] > 1) ? 1 : fades[i];
				}
				else
				{
					fades[i] -= transitionSpeed;
					fades[i] = (fades[i] < 0) ? 0 : fades[i];
				}
			}
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			scale = 1.5f;
			parallax = 0.35f;
			if (!TwilightBackgroundStyleSwap[0]){
				return BackgroundTextureLoader.GetBackgroundSlot("KingdomTerrahearts/Backgrounds/Background_TwilightTown");
			}else{
				return BackgroundTextureLoader.GetBackgroundSlot("KingdomTerrahearts/Backgrounds/Background_TwilightTown2");
			}
		}

		public override int ChooseFarTexture()
		{
			return BackgroundTextureLoader.GetBackgroundSlot("KingdomTerrahearts/Backgrounds/Background_TwilightTown5");
		}

		public override int ChooseMiddleTexture()
		{
			if (!TwilightBackgroundStyleSwap[1]){
				return BackgroundTextureLoader.GetBackgroundSlot("KingdomTerrahearts/Backgrounds/Background_TwilightTown3");
			}else{
				return BackgroundTextureLoader.GetBackgroundSlot("KingdomTerrahearts/Backgrounds/Background_TwilightTown4");
			}
		}

		public static void SetupStyleSwap(bool[] style)
		{
			for (int i = 0; i < TwilightBackgroundStyleSwap.Length && i < style.Length; i++)
			{
				TwilightBackgroundStyleSwap[i] = style[i];
			}
		}

	}

}
