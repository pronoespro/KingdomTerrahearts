using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.World.Generation;

namespace KingdomTerrahearts.Walls
{
    class TwilightWall:ModWall
    {

		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = DustID.t_Honey;
			drop = mod.ItemType("TwilightWall");
			AddMapEntry(new Color(205, 105, 0));
		}

		/*
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.4f;
			g = 0.4f;
			b = 0.4f;
		}
		*/

	}
}
