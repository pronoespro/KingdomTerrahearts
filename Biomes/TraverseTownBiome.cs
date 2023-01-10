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

namespace KingdomTerrahearts.Biomes
{
    class TraverseTownBiome : ModBiome
	{


		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Traverse Town");
		public override ModWaterStyle WaterStyle => base.WaterStyle;
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("KingdomTerrahearts/TwilightTownBackground");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Mushroom;

		// Populate the Bestiary Filter
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => Color.Blue;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Traverse Town");
		}


		public override bool IsBiomeActive(Player player)
		{
			SoraPlayer sora = player.GetModPlayer<SoraPlayer>();
			return sora.inTraverseTown;
		}

		public override void OnInBiome(Player player)
		{

		}

	}
}
