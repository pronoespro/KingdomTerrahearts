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
using KingdomTerrahearts.Extra;
using Terraria.Localization;

namespace KingdomTerrahearts
{
	public class KingdomTerrahearts : Mod
	{
        
        //Fighting gameplay
        public static ModKeybind PartySelectHotkey;
		public static ModKeybind GuardHotKey;

		//Music gameplay
		public static ModKeybind MusicUpKey;
		public static ModKeybind MusicDownKey;
		public static ModKeybind MusicLeftKey;
		public static ModKeybind MusicRightKey;

		//Costume texture slots
		public static int[] orgCoatSlots;

		//UI
		internal UserInterface partyInterface;
		internal PartyUI partyUI;

		internal UserInterface commandInterface;
		internal CommandMenu commandUI;

		internal UserInterface dialogInterface;
		internal DialogDisplay dialogUI;

		internal UserInterface levelUpInterface;
		internal KeybladeLeveling levelUpUI;

		public static KingdomTerrahearts instance;

		public static float screenShakeStrength = 1;
		public static bool keybladeThrustingEnabled;
		public static bool canDoCutscenes;

		private GameTime _LastUIUpdateGameTime;

		internal interface ILoadable
		{
			void Load();
			void Unload();
		}

		public override void Unload()
		{
			CommandLogic.instance = null;
			orgCoatSlots = new int[0];
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

			if (partyUI != null && commandUI != null && dialogUI != null)
			{
				partyUI.Destroy();
				commandUI.Destroy();
				dialogUI.Destroy();
			}

			partyUI = null;
			commandUI = null;
			dialogUI = null;
			instance = null;

		}

		public override void Load()
		{

			PartySelectHotkey = KeybindLoader.RegisterKeybind(this,"Party menu", "F");
			GuardHotKey = KeybindLoader.RegisterKeybind(this, "Guard","Q");
			MusicUpKey = KeybindLoader.RegisterKeybind(this, "MusicalUp", "Z");
			MusicLeftKey = KeybindLoader.RegisterKeybind(this, "MusicalLeft", "X");
			MusicDownKey = KeybindLoader.RegisterKeybind(this, "MusicalDown", "N");
			MusicRightKey = KeybindLoader.RegisterKeybind(this, "MusicalRight", "M");

			instance = this;

			CommandLogic.Initialize();
			CommandLogic.instance.ChangeCommand(0);

			Logger.InfoFormat("{0} Sora logging", Name);

			if (!Main.dedServ)
			{
				orgCoatSlots = new int[5];
                orgCoatSlots[0] = EquipLoader.AddEquipTexture(this, "KingdomTerrahearts/Items/Armor/orgCoat_Body", EquipType.Body,name:"orgCoatBody");
                orgCoatSlots[1] = EquipLoader.AddEquipTexture(this,  "KingdomTerrahearts/Items/Armor/orgCoat_Legs",EquipType.Legs, name: "orgCoatLegs");
				orgCoatSlots[2] = EquipLoader.AddEquipTexture(this, "KingdomTerrahearts/Items/Armor/orgCoat_Head", EquipType.Head, name: "orgCoatHood");

				orgCoatSlots[3] = EquipLoader.AddEquipTexture(this, "KingdomTerrahearts/Items/Armor/soraJacket_Body", EquipType.Body, name: "soraJacket");
				orgCoatSlots[4] = EquipLoader.AddEquipTexture(this, "KingdomTerrahearts/Items/Armor/soraClothes_Legs", EquipType.Legs, name: "soraPants");



				MusicLoader.AddMusicBox(this,
					MusicLoader.GetMusicSlot(this, "Sounds/Music/Lazy Afternoons"), ModContent.ItemType<Items.Placeable.LazyAfternoons_Item>(), ModContent.TileType<Tiles.MusicBoxes.LazyAfternoons_MusicBox>());

				MusicLoader.AddMusicBox(this,
					MusicLoader.GetMusicSlot(this, "Sounds/Music/Vector to the Heaven"), ModContent.ItemType<Items.Placeable.VectorToHeaven_Item>(), ModContent.TileType<Tiles.MusicBoxes.VectorToHeaven_MusicBox>());

			}

			if (Main.netMode != NetmodeID.Server)
			{
				// First, you load in your shader file.
				// You'll have to do this regardless of what kind of shader it is,
				// and you'll have to do it for every shader file.
				// This example assumes you have both armour and screen shaders.

				Ref<Effect> dyeRef = new Ref<Effect>(ModContent.Request<Effect>("KingdomTerrahearts/Effects/lastWorldShader").Value);

				// To add a dye, simply add this for every dye you want to add.
				// "PassName" should correspond to the name of your pass within the *technique*,
				// so if you get an error here, make sure you've spelled it right across your effect file.

				GameShaders.Armor.BindShader(ModContent.ItemType<Items.lastWorldDye>(), new ArmorShaderData(dyeRef, "ArmorMyShader"));

				Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>("KingdomTerrahearts/Effects/Shockwave").Value); // The path to the compiled shader file.
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


			//Collision extra

			On.Terraria.Player.Update_NPCCollision += CollisionDetour;

		}

		public void CollisionDetour(On.Terraria.Player.orig_Update_NPCCollision orig, Player self)
        {

			SoraPlayer sp = self.GetModPlayer<SoraPlayer>();

			if (sp.collisionDown)
			{
				if (!self.justJumped && self.velocity.Y >= 0)
				{

					self.velocity.Y = 0;
					self.fallStart = (int)(self.position.Y / 16f);
					self.position.Y = sp.collisionPoints.Y-self.height/2;
					self.legFrame.Y = 0;
					//self.position.Y = npc.position.Y - self.height + 4;
				}

			}

			if (sp.collisionUp)
			{
				if (self.velocity.Y <= 0)
				{
					//self.gfxOffY = npc.gfxOffY;
					self.velocity.Y = 0;
					self.fallStart = (int)((self.Center.Y) / 16f);
					self.position.Y = sp.collisionPoints.Y - self.height/2;
					//self.position.Y = npc.position.Y - self.height + 4;
				}

			}

			if (sp.collisionLeft)
			{
				if (self.velocity.X <= 0)
				{
					//self.gfxOffY = npc.gfxOffY;
					self.velocity.X = 0;
					//self.fallStart = (int)(self.position.Y / 16f);
					self.position.X = sp.collisionPoints.X - self.width/2;
					//self.position.Y = npc.position.Y - self.height + 4; - self.width -
				}

			}

			if (sp.collisionRight)
			{
				if (self.velocity.X >= 0)
				{
					//self.gfxOffY = npc.gfxOffY;
					self.velocity.X = 0;
					//self.fallStart = (int)(self.position.Y / 16f);
					self.position.X = sp.collisionPoints.X;
					//self.position.Y = npc.position.Y - self.height + 4; - self.width -
				}

			}
			orig(self);
		}


        public void UpdateUI(GameTime gameTime)
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

        public void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
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
			if (levelUpInterface.CurrentState == null)
			{
				levelUpInterface?.SetState(levelUpUI);
			}
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

			Recipe recipe = Recipe.Create(ItemID.FallenStar);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(),10);
			recipe.Register();

			recipe = Recipe.Create(ItemID.ShinyRedBalloon);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.lightningShard>(), 20);
			recipe.AddIngredient(ItemID.PinkGel, 10);
			recipe.AddTile(TileID.SkyMill);
			recipe.Register();


			Recipe.Create(ItemID.CloudinaBottle)
				.AddIngredient(ItemID.Cloud, 10)
				.AddIngredient(ItemID.Bottle)
				.AddTile(TileID.SkyMill)
				.Register();

			Recipe.Create(ItemID.SandstorminaBottle)
				.AddIngredient(ItemID.CloudinaBottle)
				.AddIngredient(ItemID.SandBlock,50)
				.AddTile(TileID.SkyMill)
				.Register();

			Recipe.Create(ItemID.BlizzardinaBottle)
				.AddIngredient(ItemID.CloudinaBottle)
				.AddIngredient(ItemID.IceBlock, 25)
				.AddIngredient(ItemID.SnowBlock, 25)
				.AddTile(TileID.SkyMill)
				.Register();

			recipe = Recipe.Create(ItemID.LavaCharm);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.blazingShard>(), 25);
			recipe.AddIngredient(ItemID.Obsidian, 50);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = Recipe.Create(ItemID.WaterWalkingBoots);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.frostShard>(), 25);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();

			recipe = Recipe.Create(ItemID.Starfury);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.mythrilShard>(), 20);
			recipe.AddIngredient(ItemID.FallenStar, 50);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = Recipe.Create(ItemID.Aglet);
			recipe.AddIngredient(ModContent.ItemType <Items.Materials.lightningShard>(), 10);
			recipe.AddIngredient(ItemID.IronBar, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = Recipe.Create(ItemID.AnkletoftheWind);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.lightningShard>(), 20);
			recipe.AddIngredient(ItemID.Silk, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = Recipe.Create(ItemID.WaterWalkingBoots);
			recipe.AddIngredient(ItemID.LavaCharm);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();

			recipe = Recipe.Create(ItemID.LavaCharm);
			recipe.AddIngredient(ItemID.WaterWalkingBoots);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();

			recipe = Recipe.Create(ItemID.TitaniumOre, 5);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.blazingShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.betwixtShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.lucidShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.mythrilShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.pulsingShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.lightningShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 10);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.Register();

			recipe = Recipe.Create(ModContent.ItemType<Items.Materials.twilightShard>(), 5);
			recipe.AddIngredient(ItemID.FallenStar);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = Recipe.Create(ModContent.ItemType<Items.Materials.betwixtShard>(), 5);
			recipe.AddIngredient(ItemID.IronOre);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = Recipe.Create(ModContent.ItemType<Items.Materials.betwixtShard>(), 5);
			recipe.AddIngredient(ItemID.LeadOre);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = Recipe.Create(ModContent.ItemType<Items.Materials.frostShard>(), 5);
			recipe.AddIngredient(ItemID.IceBlock);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = Recipe.Create(ItemID.Lens, 6);
			recipe.AddIngredient(ItemID.Glass);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = Recipe.Create(ItemID.MarbleBlock, 99);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = Recipe.Create(ItemID.GraniteBlock, 99);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.lucidShard>(), 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = Recipe.Create(ItemID.RainCloud, 99);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.lightningShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 10);
			recipe.AddTile(TileID.SkyMill);
			recipe.Register();

			recipe = Recipe.Create(ItemID.Cloud, 99);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 10);
			recipe.AddTile(TileID.SkyMill);
			recipe.Register();

			recipe = Recipe.Create(ItemID.SkyMill);
			recipe.AddIngredient(ModContent.ItemType <Items.Materials.lightningShard>(), 20);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 20);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddIngredient(ItemID.WaterBucket);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = Recipe.Create(ItemID.Umbrella);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = Recipe.Create(ItemID.StaffofRegrowth);
			recipe.AddIngredient(ItemID.Daybloom, 10);
			recipe.AddIngredient(ItemID.Waterleaf, 10);
			recipe.Register();

			recipe = Recipe.Create(ModContent.ItemType<Items.Materials.frostShard>(), 2);
			recipe.AddIngredient(ItemID.Penguin);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = Recipe.Create(ItemID.WoodenBoomerang);
			recipe.AddIngredient(ItemID.Wood,7);
			recipe.AddIngredient(ItemID.IronBar);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = Recipe.Create(ItemID.WoodenBoomerang);
			recipe.AddIngredient(ItemID.Wood, 7);
			recipe.AddIngredient(ItemID.LeadBar);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = Recipe.Create(ItemID.IronBar);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.betwixtShard>(),10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = Recipe.Create(ItemID.SnowBlock);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = Recipe.Create(ItemID.IceBlock);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = Recipe.Create(ItemID.PurpleIceBlock);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 2); 
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.pulsingShard>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = Recipe.Create(ItemID.PinkIceBlock);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 2); 
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe.Create(ItemID.EnchantedBoomerang, 2)
			.AddIngredient(ItemID.EnchantedBoomerang)
			.AddTile(TileID.Anvils)
			.Register();

			Recipe.Create(ItemID.DarkShard)
			.AddIngredient(ModContent.ItemType<Items.Materials.betwixtShard>(),13)
			.AddIngredient(ItemID.SoulofNight, 7)
			.AddIngredient(ItemID.DemoniteBar)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			Recipe.Create(ItemID.LightShard)
			.AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(), 13)
			.AddIngredient(ItemID.SoulofLight, 7)
			.AddIngredient(ItemID.OrichalcumBar)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			Recipe.Create(ItemID.LightShard)
			.AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(), 13)
			.AddIngredient(ItemID.SoulofLight, 7)
			.AddIngredient(ItemID.MythrilBar)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			Recipe.Create(ItemID.PinkGel,10)
			.AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(), 20)
			.AddIngredient(ItemID.Gel, 50)
			.AddTile(TileID.TinkerersWorkbench)
			.Register();

			Recipe.Create(ItemID.Wood)
			.AddIngredient(ModContent.ItemType<Items.Weapons.Joke.Keyblade_woodenStick>())
			.Register();


			Recipe.Create(ItemID.TissueSample)
			.AddIngredient(ModContent.ItemType<Items.Materials.writhingShard>(),5)
			.AddIngredient(ItemID.ShadowScale)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			Recipe.Create(ItemID.ShadowScale)
			.AddIngredient(ModContent.ItemType<Items.Materials.writhingShard>(),5)
			.AddIngredient(ItemID.TissueSample)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			Recipe.Create(ItemID.DemoniteOre)
			.AddIngredient(ModContent.ItemType<Items.Materials.writhingShard>(), 2)
			.AddIngredient(ItemID.CrimtaneOre)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			Recipe.Create(ItemID.CrimtaneOre)
			.AddIngredient(ModContent.ItemType<Items.Materials.writhingShard>(), 2)
			.AddIngredient(ItemID.DemoniteOre)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			Recipe.Create(ItemID.InfernoFork)
			//.AddCondition(new Recipe.Condition(NetworkText.FromLiteral("Journey Mode only"), (Recipe r) => Main.hardMode))
			.AddIngredient(ModContent.ItemType<Items.Materials.blazingStone>(), 2)
			.AddIngredient(ItemID.HellstoneBar)
			.AddIngredient(ItemID.DemonScythe)
			.AddTile(TileID.MythrilAnvil)
			.Register();

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

		public bool CheckIfSolidTile(int x,int y)
        {

            if (WorldGen.SolidOrSlopedTile(x, y) || WorldGen.SolidOrSlopedTile(x+1,y) || WorldGen.SolidOrSlopedTile(x,y+1) || WorldGen.SolidOrSlopedTile(x+1,y+1))
            {
				return true;
            }

			return false;
        }

		public void SetCameraForAllPlayers(Vector2 pos,float zoom=-1,float shakeForce=0,float shakeSpeed=1,float percentageChange=10)
        {
			SoraPlayer sp;
			for(int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active)
                {
					sp = Main.player[i].GetModPlayer<SoraPlayer>();
					sp.ModifyCutsceneCamera(pos, zoom, shakeForce,shakeSpeed,percentageChange);
                }
            }
        }


	}
}