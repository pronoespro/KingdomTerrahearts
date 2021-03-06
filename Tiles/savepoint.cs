﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace KingdomTerrahearts.Tiles
{
    public class savepoint:ModTile
    {

		Player player;

		public override void SetDefaults()
		{
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
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Beds };
			bed = true;
			animationFrameHeight = 38;
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

		public override bool HasSmartInteract()
		{
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.frameX == 0)
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
			Item.NewItem(i * 16, j * 16, 64, 32, ItemType<Items.Placeable.savePoint_Item>());
		}

		public override bool NewRightClick(int i, int j)
		{
			player = Main.LocalPlayer;
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
			sp.skipTime = true;
			sp.skipToDay = !Main.dayTime;
			Tile tile = Main.tile[i, j];
			int spawnX = i - tile.frameX / 18;
			int spawnY = j + 2;
			spawnX += tile.frameX >= 72 ? 5 : 2;
			if (tile.frameY % 38 != 0)
			{
				spawnY--;
			}
			player.FindSpawn();

			bool healPlayer = true;
			foreach(NPC npc in Main.npc)
			{
				if (!npc.friendly && Vector2.Distance(player.Center,npc.Center)<250)
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

			/*
			if (player.SpawnX == spawnX && player.SpawnY == spawnY)
			{
				player.RemoveSpawn();
				Main.NewText("Spawn point removed!", 255, 240, 20, false);
			}
			else if (Player.CheckSpawn(spawnX, spawnY))
			{
			*/

				player.ChangeSpawn(spawnX, spawnY);
				Main.NewText("Game Saved!", 255, 240, 20, false);

			//}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ItemType<Items.Placeable.savePoint_Item>();
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			SoraPlayer sp= new SoraPlayer();
			if (player == null)
			{
				player = Main.LocalPlayer;
				sp = player.GetModPlayer<SoraPlayer>();
			}

			Tile tile = Main.tile[i, j];
			float reallyClose = Vector2.Distance(player.position, new Vector2(i * 16, j * 16));

			if (reallyClose < 50 && player!=null)
			{
				bool healPlayer = true;
				foreach (NPC npc in Main.npc)
				{
					if (!npc.friendly && Vector2.Distance(player.Center, npc.Center) < 250)
					{
						healPlayer = false;
						break;
					}
				}
				if (healPlayer)
				{
					player.statLife = player.statLifeMax;
					if(player.statMana<player.statManaMax)
						player.statMana = player.statManaMax;
					sp.ResetTimers();
				}
			}

			base.NearbyEffects(i, j, closer);
		}

	}
}
