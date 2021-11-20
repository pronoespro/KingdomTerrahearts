using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Tiles
{
    class twilightTownBlock:ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			ItemDrop = ModContent.ItemType<Items.Placeable.twilightBlock>();
			AddMapEntry(new Color(235, 135, 0));
		}

		/*
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		*/
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0f;
			g = 0f;
			b = 0f;
		}

	}
}
