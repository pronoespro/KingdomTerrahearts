using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KingdomTerrahearts.Extra;
using Terraria.ModLoader.IO;
using System;
using Terraria.DataStructures;
using Terraria.Graphics.Capture;

namespace KingdomTerrahearts
{


    public class TwilightTownBiome:ModBiome
    {


        public override int Music => MusicLoader.GetMusicSlot(Mod,"Sounds/Music/Lazy Afternoons");
        public override ModWaterStyle WaterStyle => base.WaterStyle;
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("KingdomTerrahearts/TwilightTownBackground");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

		// Populate the Bestiary Filter
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => Color.Orange;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Town");
		}


		public override bool IsBiomeActive(Player player)
        {
			SoraPlayer sora = player.GetModPlayer<SoraPlayer>();
            return sora.inTwilightTown;
        }

        public override void OnInBiome(Player player)
        {

			if (KingdomTerrahearts.instance.AnyEnemiesAround())
			{
				if (!MusicLoader.GetMusic("KingdomTerrahearts/Sounds/Music/Twilight Town Combat").IsPlaying)
				{
					MusicLoader.GetMusic("KingdomTerrahearts/Sounds/Music/Twilight Town Combat").Play();
				}
			}
			else
			{
				if (!MusicLoader.GetMusic("KingdomTerrahearts/Sounds/Music/Lazy Afternoons").IsPlaying)
				{
					MusicLoader.GetMusic("KingdomTerrahearts/Sounds/Music/Lazy Afternoons").Play();
				}
			}
        }

    }
}
