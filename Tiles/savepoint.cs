using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace KingdomTerrahearts.Tiles
{
    public class savepoint:ModTile
    {

		Player player;

		public override void SetStaticDefaults()
		{
			TileID.Sets.IsValidSpawnPoint[Type] = true;
			
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); //this style already takes care of direction for us
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Save point");
			AddMapEntry(new Color(200, 200, 200), name);
			//dustType = DustType<Sparkle>();

			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.Beds };
			AnimationFrameHeight = 38;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			// Flips the sprite if x coord is odd. Makes the tile more interesting.
			if (j % 4 > 2)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}


		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 8)
			{
				frameCounter = 0;
				frame++;
				if (frame >= 4)
				{
					frame = 0;
				}
			}
		}
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 0)
			{
				// We can support different light colors for different styles here: switch (tile.frameY / 54)
				r = 0.75f;
				g = 0.75f;
				b = 2f;
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{

			player = Main.LocalPlayer;
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

			EntitySource_TileBreak s = new EntitySource_TileBreak(i, j);

			Item.NewItem(s,i * 16, j * 16, 64, 32, ItemType<Items.Placeable.savePoint_Item>());
		}

        public override bool RightClick(int i, int j)
		{

			player = Main.LocalPlayer;
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

			Tile tile = Main.tile[i, j];
			int spawnX = i - tile.TileFrameX / 18;
			int spawnY = j + 2;
			spawnX += tile.TileFrameX >= 72 ? 5 : 2;

			if (tile.TileFrameY % 38 != 0)
			{
				spawnY--;
			}
			player.FindSpawn();

			player.ChangeSpawn(spawnX, spawnY);
			Main.NewText("Game Saved!", 255, 240, 20);

			bool healPlayer = true;
			foreach (NPC npc in Main.npc)
			{
				if (!npc.friendly && !npc.friendly && !npc.CountsAsACritter && Vector2.Distance(player.Center, npc.Center) < 250)
				{
					healPlayer = false;
					break;
				}
			}
			if (healPlayer)
			{
				player.statLife = player.statLifeMax;
				player.statMana = player.statManaMax;
				sp.ResetTimers();
			}


			if (player.SpawnX == spawnX && player.SpawnY == spawnY)
			{
				sp.skipTime = true;
				sp.skipToDay = !Main.dayTime;
			}


			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ItemType<Items.Placeable.savePoint_Item>();
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			SoraPlayer sp= new SoraPlayer();
			if (player == null)
			{
				player = Main.LocalPlayer;
				sp = player.GetModPlayer<SoraPlayer>();
			}

			float reallyClose = Vector2.Distance(player.position, new Vector2(i * 16, j * 16));

			if (reallyClose < 750 && player!=null)
			{

				sp.canTPToSavePoints = true;


				bool healPlayer = true;
				foreach (NPC npc in Main.npc)
				{
					if (!npc.townNPC && !npc.friendly && !npc.CountsAsACritter && Vector2.Distance(player.Center, npc.Center) < 250)
					{
						healPlayer = false;
						break;
					}
				}

				if (healPlayer)
				{
					player.statLife = player.statLifeMax;
					if (player.statMana < player.statManaMax)
					{
						player.statMana = player.statManaMax;
					}

					sp.ResetTimers();
				}
            }
		}

	}
}
