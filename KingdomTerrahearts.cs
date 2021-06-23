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
using System;

namespace KingdomTerrahearts
{
	public class KingdomTerrahearts : Mod
	{

		//Fighting gameplay
		public static ModHotKey PartySelectHotkey;
		public static ModHotKey GuardHotKey;

		//Music gameplay
		public static ModHotKey MusicUpKey;
		public static ModHotKey MusicDownKey;
		public static ModHotKey MusicLeftKey;
		public static ModHotKey MusicRightKey;

		internal UserInterface partyInterface;
		internal PartyUI partyUI;

		internal UserInterface commandInterface;
		internal CommandMenu commandUI;

		internal UserInterface dialogInterface;
		internal DialogDisplay dialogUI;

		internal UserInterface levelUpInterface;
		internal KeybladeLeveling levelUpUI;

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
			HidePartyUI();
			HideCommandUI();
			HideDialogUI();
			HideLevelUpUI();

			PartySelectHotkey = null;
			GuardHotKey = null;
			MusicUpKey = null;
			MusicLeftKey = null;
			MusicDownKey = null;
			MusicRightKey = null;

			partyUI.Destroy();
			commandUI.Destroy();
			dialogUI.Destroy();
			partyUI = null;
			commandUI = null;
			dialogUI = null;
			instance = null;
		}

		public override void Load()
		{
			PartySelectHotkey = RegisterHotKey("Party menu", "F");
			GuardHotKey = RegisterHotKey("Guard","Q");
			MusicUpKey = RegisterHotKey("MusicalUp", "Z");
			MusicLeftKey = RegisterHotKey("MusicalLeft", "X");
			MusicDownKey = RegisterHotKey("MusicalDown", "N");
			MusicRightKey = RegisterHotKey("MusicalRight", "M");

			instance = this;

			CommandLogic.Initialize();
			CommandLogic.instance.ChangeCommand(0);

			Logger.InfoFormat("{0} Sora logging", Name);

			if (!Main.dedServ)
			{
				AddEquipTexture(null, EquipType.Legs, "orgCoatLegs", "KingdomTerrahearts/Items/Armor/orgCoat_Legs");
				AddEquipTexture(new Items.Armor.orgCoatHead(), null, EquipType.Head, "orgCoatHead", "KingdomTerrahearts/Items/Armor/orgCoat_Head");
				AddEquipTexture(new Items.Armor.orgCoatBody(), null, EquipType.Body, "orgCoatBody", "KingdomTerrahearts/Items/Armor/orgCoat_Body", "KingdomTerrahearts/Items/Armor/orgCoat_Arms");
				AddEquipTexture(new Items.Armor.orgCoatLegs(), null, EquipType.Legs, "orgCoatLegs", "KingdomTerrahearts/Items/Armor/orgCoat_Legs");

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Lazy Afternoons"), ItemType("LazyAfternoons_Item"), TileType("LazyAfternoons_MusicBox"));

				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Vector to the Heaven"), ItemType("VectorToHeaven_Item"), TileType("VectorToHeaven_MusicBox"));
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

				Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/Shockwave")); // The path to the compiled shader file.
				Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["Shockwave"].Load();
			}

            if (!Main.dedServ)
            {
				partyInterface = new UserInterface();
				partyUI = new PartyUI();
				partyUI.Activate();

				commandInterface = new UserInterface();
				commandUI = new CommandMenu();
				commandUI.Activate();

				dialogInterface = new UserInterface();
				dialogUI = new DialogDisplay();
				dialogUI.Activate();

				levelUpInterface = new UserInterface();
				levelUpUI = new KeybladeLeveling();
				levelUpUI.Activate();

			}
		}

		public override void UpdateUI(GameTime gameTime)
        {
			_LastUIUpdateGameTime = gameTime;
            if (partyInterface?.CurrentState != null)
            {
				partyInterface.Update(gameTime);
			}
			if (commandInterface?.CurrentState != null)
			{
				commandInterface.Update(gameTime);
			}
			if (dialogInterface?.CurrentState != null)
			{
				dialogInterface.Update(gameTime);
			}
			if (levelUpInterface?.CurrentState != null)
			{
				levelUpInterface.Update(gameTime);
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
						if (_LastUIUpdateGameTime != null)
						{
							if(partyInterface?.CurrentState != null) 
							partyInterface.Draw(Main.spriteBatch, _LastUIUpdateGameTime);


							if (commandInterface?.CurrentState != null)
								commandInterface.Draw(Main.spriteBatch, _LastUIUpdateGameTime);

							if (dialogInterface?.CurrentState != null)
								dialogInterface.Draw(Main.spriteBatch, _LastUIUpdateGameTime);

							if (levelUpInterface?.CurrentState != null)
								levelUpInterface.Draw(Main.spriteBatch, _LastUIUpdateGameTime);
						}
						return true;
					},
					   InterfaceScaleType.UI));
			}
		}

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			if (!Main.gameMenu)
			{
				if (Main.invasionX == Main.spawnTileX && KingdomWorld.customInvasionUp)
				{
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SinesterShadows");
				}
				if (Main.LocalPlayer.GetModPlayer<SoraPlayer>().inTwilightTown)
				{
					if (AnyEnemiesAround())
						music = GetSoundSlot(SoundType.Music, "Sounds/Music/Twilight Town Combat");
					else
						music = GetSoundSlot(SoundType.Music, "Sounds/Music/Lazy Afternoons");
					priority = MusicPriority.BiomeMedium;
				}
			}

			base.UpdateMusic(ref music, ref priority);

		}

		public bool AnyEnemiesAround()
        {
			for(int i = 0; i < Main.maxNPCs; i++)
            {
				if (Main.npc[i].active && ((!Main.npc[i].friendly && Main.npc[i].damage>0)||Main.npc[i].boss))
				{
					if (Vector2.Distance(Main.npc[i].Center, Main.LocalPlayer.Center) < 750)
						return true;
				}
            }
			return false;
        }

		public bool IsEnemy(int enemy)
        {
			NPC e = Main.npc[enemy];
			return e.active && ((!e.friendly && e.damage > 0) || e.boss);
        }

		public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
		{
			if (KingdomWorld.twilightBiome <= 0)
			{
				return;
			}

			float sunStrength = KingdomWorld.twilightBiome / 225f;
			sunStrength = Math.Min(sunStrength, 1f);

			int sunR = backgroundColor.R;
			int sunG = backgroundColor.G;
			int sunB = backgroundColor.B;
			// Remove some green and more red.
			sunR -= (int)(15f * sunStrength * (backgroundColor.R / 255f));
			sunG -= (int)(75f * sunStrength * (backgroundColor.G / 255f));
			sunB -= (int)(115f * sunStrength * (backgroundColor.B / 255f));
			sunR = Utils.Clamp(sunR, 15, 255);
			sunG = Utils.Clamp(sunG, 15, 255);
			sunB = Utils.Clamp(sunB, 15, 255);
			backgroundColor.R = (byte)sunR;
			backgroundColor.G = (byte)sunG;
			backgroundColor.B = (byte)sunB;
		}


		internal void ShowCommandUI()
        {
			commandInterface?.SetState(commandUI);
		}

		internal void HideCommandUI()
		{
			commandInterface?.SetState(null);
		}

		internal void ShowPartyUI()
		{
			partyInterface?.SetState(partyUI);
		}

		internal void HidePartyUI()
		{
			partyInterface?.SetState(null);
		}

		internal void ShowDialogUI()
        {
			dialogInterface?.SetState(dialogUI);
        }

		internal void HideDialogUI()
		{
			dialogInterface?.SetState(null);
		}

		internal void ShowLevelUpUI()
        {
			levelUpInterface?.SetState(levelUpUI);
        }

		internal void HideLevelUpUI()
		{
			levelUpInterface?.SetState(null);
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

        public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("twilightShard"),10);
			recipe.SetResult(ItemID.FallenStar);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("blazingShard"), 25);
			recipe.AddIngredient(ItemID.Obsidian, 50);
			recipe.SetResult(ItemID.LavaCharm);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("frostShard"), 25);
			recipe.SetResult(ItemID.WaterWalkingBoots);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("powerShard"), 20);
			recipe.AddIngredient(ItemID.FallenStar, 50);
			recipe.SetResult(ItemID.Starfury);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("thunderShard"), 10);
			recipe.AddIngredient(ItemID.IronBar, 1);
			recipe.SetResult(ItemID.Aglet);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("thunderShard"), 20);
			recipe.AddIngredient(ItemID.Silk, 20);
			recipe.SetResult(ItemID.AnkletoftheWind);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.LavaCharm);
			recipe.SetResult(ItemID.WaterWalkingBoots);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.WaterWalkingBoots);
			recipe.SetResult(ItemID.LavaCharm);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("blazingShard"), 10);
			recipe.AddIngredient(ItemType("denseShard"), 10);
			recipe.AddIngredient(ItemType("frostShard"), 10);
			recipe.AddIngredient(ItemType("lucidShard"), 10);
			recipe.AddIngredient(ItemType("powerShard"), 10);
			recipe.AddIngredient(ItemType("pulsingShard"), 10);
			recipe.AddIngredient(ItemType("thunderShard"), 10);
			recipe.AddIngredient(ItemType("twilightShard"), 10);
			recipe.SetResult(ItemID.TitaniumOre,5);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.FallenStar);
			recipe.SetResult(ItemType("twilightShard"),5);
			recipe.AddTile(TileID.Furnaces);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.IronOre);
			recipe.SetResult(ItemType("denseShard"), 5);
			recipe.AddTile(TileID.Furnaces);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.LeadOre);
			recipe.SetResult(ItemType("denseShard"), 5);
			recipe.AddTile(TileID.Furnaces);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.IceBlock);
			recipe.SetResult(ItemType("frostShard"), 5);
			recipe.AddTile(TileID.Furnaces);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Glass);
			recipe.SetResult(ItemID.Lens, 6);
			recipe.AddTile(TileID.Furnaces);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("twilightShard"), 10);
			recipe.SetResult(ItemID.MarbleBlock, 99);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("lucidShard"), 10);
			recipe.SetResult(ItemID.GraniteBlock, 99);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("thunderShard"), 10);
			recipe.AddIngredient(ItemType("twilightShard"), 10);
			recipe.SetResult(ItemID.RainCloud, 99);
			recipe.AddTile(TileID.SkyMill);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("twilightShard"), 10);
			recipe.SetResult(ItemID.Cloud,99);
			recipe.AddTile(TileID.SkyMill);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("thunderShard"), 20);
			recipe.AddIngredient(ItemID.PinkGel, 10);
			recipe.SetResult(ItemID.ShinyRedBalloon);
			recipe.AddTile(TileID.SkyMill);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("thunderShard"), 20);
			recipe.AddIngredient(ItemType("twilightShard"), 20);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddIngredient(ItemID.WaterBucket);
			recipe.SetResult(ItemID.SkyMill);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.SetResult(ItemID.Umbrella);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Daybloom, 10);
			recipe.AddIngredient(ItemID.Waterleaf, 10);
			recipe.SetResult(ItemID.StaffofRegrowth);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Penguin);
			recipe.SetResult(ItemType("frostShard"),2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Wood,7);
			recipe.AddIngredient(ItemID.IronBar);
			recipe.SetResult(ItemID.WoodenBoomerang);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Wood, 7);
			recipe.AddIngredient(ItemID.LeadBar);
			recipe.SetResult(ItemID.WoodenBoomerang);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.WoodenBoomerang);
			recipe.AddIngredient(ItemID.FallenStar);
			recipe.SetResult(ItemID.EnchantedBoomerang);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemType("denseShard"),10);
			recipe.SetResult(ItemID.IronBar);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();

		}

		public bool AnyProjectile(int type)
        {
			for(int i = 0; i < Main.maxProjectiles; i++)
            {
				if(Main.projectile[i].active && Main.projectile[i].type == type)
                {
					return true;
                }
            }
			return false;
        }

	}
}