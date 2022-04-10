using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;
using Terraria.UI;
using Terraria.GameContent.ObjectInteractions;

namespace KingdomTerrahearts.Tiles
{

	public class WeaponToDisplay
    {
		public string weaponSprite;
		public int weaponType;

		public WeaponToDisplay(string sprite,int type)
        {
			weaponSprite = sprite;
			weaponType = type;
        }
    }

    public class WeaponDisplay: ModTile
    {

        public static int displayNum = 0;

		public Vector2 weaponPos;
		public float weaponMoveSpeed;
		public float weaponMoveMagnitude;

		public WeaponToDisplay heldItem;
		public bool displayedItem;


		private readonly int _context;

		public override void SetStaticDefaults()
		{

			Main.tileLighted[Type] = true;
			Main.tileBlockLight[Type] = false;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			Main.tileObsidianKill[Type] = false;

			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);

			TileObjectData.newTile.Width = 5;
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.CoordinateHeights = new int[5];
			for (int i = 0; i < TileObjectData.newTile.Height; i++)
			{
				TileObjectData.newTile.CoordinateHeights[i] = 16;
			}
			TileObjectData.newTile.Origin = new Point16(2, 4);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 5, 0 );
			TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			TileObjectData.newTile.AnchorLeft = AnchorData.Empty;
			TileObjectData.newTile.AnchorRight = AnchorData.Empty;

			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Weapon Display");
			AddMapEntry(Color.White, name);

			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { };

		}

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			displayedItem = false;
			if (weaponPos == Vector2.Zero)
			{
				weaponPos = new Vector2(i, j)*16;
			}
			if (heldItem == null)
			{
				switch (displayNum)
				{
					default:
						break;
					case 0:
						heldItem = new WeaponToDisplay("KingdomTerrahearts/Items/Weapons/Joke/dreamRod", ModContent.ItemType<Items.Weapons.Joke.dreamRod>());
						break;
					case 1:
						heldItem = new WeaponToDisplay("KingdomTerrahearts/Items/Weapons/Joke/dreamShield", ModContent.ItemType<Items.Weapons.Joke.dreamShield>());
						break;
					case 2:
						heldItem = new WeaponToDisplay("KingdomTerrahearts/Items/Weapons/Joke/dreamSword", ModContent.ItemType<Items.Weapons.Joke.dreamSword>());
						break;
				}
				displayNum++;
			}
			return base.PreDraw(i, j, spriteBatch);
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
				r = 1f;
				g = 1f;
				b = 1f;
			}
		}

		public void ChangePlacedItem(WeaponToDisplay weapon)
        {
			heldItem = weapon;
        }

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			EntitySource_TileInteraction s = new EntitySource_TileInteraction(player,i, j);
			Vector2 pos = new Vector2(i, j) * 16;
            if (heldItem != null)
            {
				Item.NewItem(s,pos, heldItem.weaponType);
				heldItem = null;
            }
            else
			{
				heldItem = new WeaponToDisplay("KingdomTerrahearts/Items/Weapons/Joke/dreamSword", ModContent.ItemType<Items.Weapons.Joke.dreamSword>());
			}

			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ItemType<Items.Placeable.WeaponDisplayItem>();
		}

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {

			Point p = new Point(i, j);
			Main.instance.TilesRenderer.AddSpecialLegacyPoint(p);

            base.DrawEffects(i, j, spriteBatch, ref drawData);
        }

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (heldItem != null && !displayedItem)
			{
				spriteBatch.Draw(ModContent.Request<Texture2D>(heldItem.weaponSprite).Value, weaponPos, Color.White);
				displayedItem = true;
			}
			base.SpecialDraw(i, j, spriteBatch);
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
			return true;
		}

		public override bool CanExplode(int i, int j)
		{
			return false;
		}


	}
}
