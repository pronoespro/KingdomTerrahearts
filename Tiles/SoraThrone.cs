using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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

		public override void SetDefaults()
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
			dustType = DustID.Gold;
			disableSmartCursor = false;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("SoraThrone_Item"));
		}

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
			Vector2 pos = new Vector2(i * 16, j * 16);
			if (Main.rand.Next(10) == 0)
			{
				int dust= Dust.NewDust(pos, 16, 16, DustID.CrystalPulse);
				Main.dust[dust].alpha = 230;
				Main.dust[dust].scale *= 0.4f;
			}
            base.DrawEffects(i, j, spriteBatch, ref drawColor, ref nextSpecialDrawIndex);
        }

        public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("SoraThrone_Item");
		}

        public override bool NewRightClick(int i, int j)
		{
			player = Main.LocalPlayer;
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

			sp.levelUpShowingTime =(sp.levelUpShowingTime>0)? 0:5;

			return true;
        }

        public override void NearbyEffects(int i, int j, bool closer)
		{
			player = Main.LocalPlayer;
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
			if (!closer && sp.levelUpShowingTime > 0)
				sp.levelUpShowingTime = 0;
        }

    }
}
