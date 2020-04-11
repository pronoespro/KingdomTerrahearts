using Terraria.ModLoader;

namespace KingdomTerrahearts
{
	public class KingdomTerrahearts : Mod
	{

		public KingdomTerrahearts instance;

		internal interface ILoadable
		{
			void Load();
			void Unload();
		}

		public KingdomTerrahearts()
		{

			instance = this;
			

		}

		public override void Unload()
		{
			instance = null;
		}


	}
}