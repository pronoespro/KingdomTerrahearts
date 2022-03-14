using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;

namespace KingdomTerrahearts.Tiles
{
    public class SoraThrone:ModTile
	{

		Player player;

		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.CoordinateHeights = new[] { 16 , 16 , 16 ,16};
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Dark Throne");
			AddMapEntry(new Color(100, 100, 100), name);
			DustType = DustID.Gold;
			TileID.Sets.DisableSmartCursor[Type] = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			EntitySource_TileBreak s = new EntitySource_TileBreak(i, j);
			Item.NewItem(s,i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Placeable.SoraThrone_Item>());
		}

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			Vector2 pos = new Vector2(i * 16, j * 16);
			if (Main.rand.Next(10) == 0)
			{
				int dust = Dust.NewDust(pos, 16, 16, DustID.CrystalPulse);
				Main.dust[dust].alpha = 230;
				Main.dust[dust].scale *= 0.4f;
			}
			base.DrawEffects(i, j, spriteBatch, ref drawData);
        }

        public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.SoraThrone_Item>();
		}

        public override bool RightClick(int i, int j)
		{
			player = Main.LocalPlayer;
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

			sp.levelUpShowingTime =15;

			return true;
        }

        public override void NearbyEffects(int i, int j, bool closer)
		{
			player = Main.LocalPlayer;
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

			Vector2 tilePos = new Vector2(i, j) * 16;

            if (Vector2.Distance(player.Center, tilePos) > 100)
            {
				sp.levelUpShowingTime = 0;
            }

        }

    }
}
