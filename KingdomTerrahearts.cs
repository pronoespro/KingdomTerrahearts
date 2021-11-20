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
				orgCoatSlots = new int[3];
				orgCoatSlots[0] = AddEquipTexture(new Items.Armor.orgCoat(), EquipType.Body, "KingdomTerrahearts/Items/Armor/orgCoat_Body");
				orgCoatSlots[1] = AddEquipTexture(new Items.Armor.orgCoat(), EquipType.Legs, "KingdomTerrahearts/Items/Armor/orgCoat_Legs");
				orgCoatSlots[2] = AddEquipTexture(new Items.Armor.orgCoat(), EquipType.Head, "KingdomTerrahearts/Items/Armor/orgCoat_Head");



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
					self.position.Y = sp.collisionPoints.Y;
					//self.position.Y = npc.position.Y - self.height + 4;
					// orig(self);
				}

			}

			if (sp.collisionUp)
			{
				if (self.velocity.Y <= 0)
				{
					//self.gfxOffY = npc.gfxOffY;
					self.velocity.Y = 0;
					self.fallStart = (int)(self.position.Y / 16f);
					self.position.Y = sp.collisionPoints.Y - self.height/2;
					//self.position.Y = npc.position.Y - self.height + 4;
					// orig(self);
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
					// orig(self);
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
					// orig(self);
				}

			}

			orig(self);
		}

        public override void PostSetupContent()
        {
            base.PostSetupContent();
			/*
			Mod BossChecklist = ModLoader.GetMod("BossChecklist");

			if (BossChecklist != null)
			{
				BossChecklist.Call("AddBossWithInfo", "Darkside", 0.5f, (Func<bool>)(() => KingdomWorld.downedDarkside), "Use a [i:" + ModContent.ItemType<Items.DarkenedHeart>() + "]");
				BossChecklist.Call("AddBossWithInfo", "1000 heartless battle", 0.5f, (Func<bool>)(() => KingdomWorld.downedCustomInvasion), "Use a [i:" + ModContent.ItemType<NPCs.Invasions.ThousandHearlessBattleSpawner>() + "]");
				BossChecklist.Call("AddBossWithInfo", "Xion Phase 1", 0.5f, (Func<bool>)(() => KingdomWorld.downedXionPhases[0]), "Use a [i:" + ModContent.ItemType<Items.seasaltIcecream>() + "]");
				BossChecklist.Call("AddBossWithInfo", "Xion Phase 2", 6.5f, (Func<bool>)(() => KingdomWorld.downedXionPhases[1]), "Use a [i:" + ModContent.ItemType<Items.seasaltIcecream>() + "] in Hardmode");
				BossChecklist.Call("AddBossWithInfo", "Xion Phase 3", 15f, (Func<bool>)(() => KingdomWorld.downedXionPhases[2]), "Use a [i:" + ModContent.ItemType<Items.seasaltIcecream>() + "] after beating the Moonlord");
			}
			*/
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

			Recipe recipe = CreateRecipe(ItemID.FallenStar);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(),10);
			recipe.Register();

			recipe = CreateRecipe(ItemID.ShinyRedBalloon);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.thunderShard>(), 20);
			recipe.AddIngredient(ItemID.PinkGel, 10);
			recipe.AddTile(TileID.SkyMill);
			recipe.Register();

			CreateRecipe(ItemID.CloudinaBottle)
				.AddIngredient(ItemID.Cloud, 10)
				.AddIngredient(ItemID.Bottle)
				.AddTile(TileID.SkyMill)
				.Register();

			CreateRecipe(ItemID.SandstorminaBottle)
				.AddIngredient(ItemID.CloudinaBottle)
				.AddIngredient(ItemID.SandBlock,50)
				.AddTile(TileID.SkyMill)
				.Register();

			CreateRecipe(ItemID.BlizzardinaBottle)
				.AddIngredient(ItemID.CloudinaBottle)
				.AddIngredient(ItemID.IceBlock, 25)
				.AddIngredient(ItemID.SnowBlock, 25)
				.AddTile(TileID.SkyMill)
				.Register();

			recipe = CreateRecipe(ItemID.LavaCharm);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.blazingShard>(), 25);
			recipe.AddIngredient(ItemID.Obsidian, 50);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = CreateRecipe(ItemID.WaterWalkingBoots);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.frostShard>(), 25);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();

			recipe = CreateRecipe(ItemID.Starfury);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.powerShard>(), 20);
			recipe.AddIngredient(ItemID.FallenStar, 50);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = CreateRecipe(ItemID.Aglet);
			recipe.AddIngredient(ModContent.ItemType <Items.Materials.thunderShard>(), 10);
			recipe.AddIngredient(ItemID.IronBar, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = CreateRecipe(ItemID.AnkletoftheWind);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.thunderShard>(), 20);
			recipe.AddIngredient(ItemID.Silk, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = CreateRecipe(ItemID.WaterWalkingBoots);
			recipe.AddIngredient(ItemID.LavaCharm);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();

			recipe = CreateRecipe(ItemID.LavaCharm);
			recipe.AddIngredient(ItemID.WaterWalkingBoots);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();

			recipe = CreateRecipe(ItemID.TitaniumOre, 5);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.blazingShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.denseShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.lucidShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.powerShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.pulsingShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.thunderShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 10);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.Register();

			recipe = CreateRecipe(ModContent.ItemType<Items.Materials.twilightShard>(), 5);
			recipe.AddIngredient(ItemID.FallenStar);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = CreateRecipe(ModContent.ItemType<Items.Materials.denseShard>(), 5);
			recipe.AddIngredient(ItemID.IronOre);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = CreateRecipe(ModContent.ItemType<Items.Materials.denseShard>(), 5);
			recipe.AddIngredient(ItemID.LeadOre);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = CreateRecipe(ModContent.ItemType<Items.Materials.frostShard>(), 5);
			recipe.AddIngredient(ItemID.IceBlock);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = CreateRecipe(ItemID.Lens, 6);
			recipe.AddIngredient(ItemID.Glass);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();

			recipe = CreateRecipe(ItemID.MarbleBlock, 99);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = CreateRecipe(ItemID.GraniteBlock, 99);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.lucidShard>(), 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = CreateRecipe(ItemID.RainCloud, 99);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.thunderShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 10);
			recipe.AddTile(TileID.SkyMill);
			recipe.Register();

			recipe = CreateRecipe(ItemID.Cloud, 99);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 10);
			recipe.AddTile(TileID.SkyMill);
			recipe.Register();

			recipe = CreateRecipe(ItemID.SkyMill);
			recipe.AddIngredient(ModContent.ItemType <Items.Materials.thunderShard>(), 20);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 20);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddIngredient(ItemID.WaterBucket);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = CreateRecipe(ItemID.Umbrella);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = CreateRecipe(ItemID.StaffofRegrowth);
			recipe.AddIngredient(ItemID.Daybloom, 10);
			recipe.AddIngredient(ItemID.Waterleaf, 10);
			recipe.Register();

			recipe = CreateRecipe(ModContent.ItemType<Items.Materials.frostShard>(), 2);
			recipe.AddIngredient(ItemID.Penguin);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			recipe = CreateRecipe(ItemID.WoodenBoomerang);
			recipe.AddIngredient(ItemID.Wood,7);
			recipe.AddIngredient(ItemID.IronBar);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = CreateRecipe(ItemID.WoodenBoomerang);
			recipe.AddIngredient(ItemID.Wood, 7);
			recipe.AddIngredient(ItemID.LeadBar);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = CreateRecipe(ItemID.IronBar);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.denseShard>(),10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = CreateRecipe(ItemID.SnowBlock);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = CreateRecipe(ItemID.IceBlock);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = CreateRecipe(ItemID.PurpleIceBlock);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 2); 
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.pulsingShard>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			recipe = CreateRecipe(ItemID.PinkIceBlock);
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.frostShard>(), 2); 
			recipe.AddIngredient(ModContent.ItemType < Items.Materials.twilightShard>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			CreateRecipe(ItemID.EnchantedBoomerang, 2)
			.AddIngredient(ItemID.EnchantedBoomerang)
			.AddTile(TileID.Anvils)
			.Register();

			CreateRecipe(ItemID.DarkShard)
			.AddIngredient(ModContent.ItemType<Items.Materials.denseShard>(),13)
			.AddIngredient(ItemID.SoulofNight, 7)
			.AddIngredient(ItemID.DemoniteBar)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			CreateRecipe(ItemID.LightShard)
			.AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(), 13)
			.AddIngredient(ItemID.SoulofLight, 7)
			.AddIngredient(ItemID.OrichalcumBar)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			CreateRecipe(ItemID.LightShard)
			.AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(), 13)
			.AddIngredient(ItemID.SoulofLight, 7)
			.AddIngredient(ItemID.MythrilBar)
			.AddTile(TileID.MythrilAnvil)
			.Register();

			CreateRecipe(ItemID.PinkGel,10)
			.AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(), 20)
			.AddIngredient(ItemID.Gel, 50)
			.AddTile(TileID.TinkerersWorkbench)
			.Register();

			CreateRecipe(ItemID.Wood)
			.AddIngredient(ModContent.ItemType<Items.Weapons.Joke.Keyblade_woodenStick>())
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


	}
}