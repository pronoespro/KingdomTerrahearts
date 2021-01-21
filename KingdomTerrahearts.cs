using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using KingdomTerrahearts.Interface;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KingdomTerrahearts
{
	public class KingdomTerrahearts : Mod
	{

		public static ModHotKey PartySelectHotkey;

		internal UserInterface partyInterface;
		internal PartyUI partyUI;

		public static KingdomTerrahearts instance;

		private GameTime _LastUIUpdateGameTime;

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
			PartySelectHotkey = null;
			partyUI.Destroy();
			partyUI = null;
			instance = null;
		}

		public override void Load()
		{
			PartySelectHotkey = RegisterHotKey("Party menu", "F");

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

            if (!Main.dedServ)
            {
				partyInterface = new UserInterface();
				partyUI = new PartyUI();
				partyUI.Activate();
            }
		}

        public override void UpdateUI(GameTime gameTime)
        {
			_LastUIUpdateGameTime = gameTime;
            if (partyInterface?.CurrentState != null)
            {
				partyInterface.Update(gameTime);
            }
        }

		

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1)
			{
				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"KingdomTerrahearts: PartyInterface",
					delegate
					{
						if (_LastUIUpdateGameTime != null && partyInterface?.CurrentState != null)
						{
							partyInterface.Draw(Main.spriteBatch, _LastUIUpdateGameTime);
						}
						return true;
					},
					   InterfaceScaleType.UI));
			}
		}

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			if (Main.invasionX == Main.spawnTileX && KingdomWorld.customInvasionUp)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/SinesterShadows");

			}


		}

		internal void ShowPartyUI()
		{
			partyInterface?.SetState(partyUI);
		}

		internal void HidePartyUI()
		{
			partyInterface?.SetState(null);
		}

		internal void TogglePartyUI()
		{
			partyInterface?.SetState((partyInterface?.CurrentState == partyUI)?null: partyUI);
			if(partyInterface?.CurrentState == partyUI)
				Main.playerInventory = true;
		}

		internal void SetPartyUI(bool setUI)
		{
			partyInterface?.SetState((!setUI) ? null : partyUI);
		}

	}
}