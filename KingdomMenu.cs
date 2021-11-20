using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace KingdomTerrahearts
{
	public class KingdomMenu : ModMenu
	{

		private const string menuAssetPath = "KingdomTerrahearts/Menu"; // Creates a constant variable representing the texture path, so we don't have to write it out multiple times

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/KingdomTerraheartsLogo");

		//public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/ExampleSun");

		public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/KingdomHeartsMoon");

		public override int Music => MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/MainMenu");

		public override string DisplayName => "Kingdom Terrahearts";

		public override void OnSelected()
		{
			SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/keybladeBlocking")); // Plays a thunder sound when this ModMenu is selected
		}

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			logoRotation = 0;
			drawColor = Color.White; // Changes the draw color of the logo
			logoScale += 0.2f;
			return true;
		}

        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("KingdomTerrahearts/TwilightTownBackground");


    }
}
