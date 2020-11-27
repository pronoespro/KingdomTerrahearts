using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;

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



		}

		public override void Unload()
		{
			instance = null;
		}

		public override void Load()
		{
			instance = this;
			Logger.InfoFormat("{0} Sora logging", Name);

			if (!Main.dedServ)
			{
				AddEquipTexture(null, EquipType.Legs, "orgCoatLegs", "KingdomTerrahearts/Items/Armor/orgCoat_Legs");
				AddEquipTexture(new Items.Armor.orgCoatHead(), null, EquipType.Head, "orgCoatHead", "KingdomTerrahearts/Items/Armor/orgCoat_Head");
				AddEquipTexture(new Items.Armor.orgCoatBody(), null, EquipType.Body, "orgCoatBody", "KingdomTerrahearts/Items/Armor/orgCoat_Body", "KingdomTerrahearts/Items/Armor/orgCoat_Arms");
				AddEquipTexture(new Items.Armor.orgCoatLegs(), null, EquipType.Legs, "orgCoatLegs", "KingdomTerrahearts/Items/Armor/orgCoat_Legs");
			}

			if (Main.netMode != NetmodeID.Server)
			{
				// First, you load in your shader file.
				// You'll have to do this regardless of what kind of shader it is,
				// and you'll have to do it for every shader file.
				// This example assumes you have both armour and screen shaders.

				Ref<Effect> dyeRef = new Ref<Effect>(GetEffect("Effects/lastWorldShader"));

				// To add a dye, simply add this for every dye you want to add.
				// "PassName" should correspond to the name of your pass within the *technique*,
				// so if you get an error here, make sure you've spelled it right across your effect file.

				GameShaders.Armor.BindShader(ItemType("lastWorldDye"), new ArmorShaderData(dyeRef, "ArmorMyShader"));
			}
		}

		public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			if (Main.invasionX == Main.spawnTileX && KingdomWorld.customInvasionUp)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/SinesterShadows");

			}


		}
	}
}