using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Logic
{

    public class DiscoveredSavePoint
    {
        public int tileX { get; private set; }=0;
        public int tileY { get; private set; }=0;

        public DiscoveredSavePoint(int tX, int tY)
        {
            tileX = tX;
            tileY = tY;
        }

        public static bool Equals(DiscoveredSavePoint sp1,DiscoveredSavePoint sp2)
        {

            if (sp1.tileX != sp2.tileX || sp1.tileY!=sp2.tileY)
            {
                return false;
            }

            return true;
        }

        public Vector2 ToWorldPos()
        {
            return new Vector2(tileX * 16, tileY * 16);
        }

    }

    public class SavePointTravelingRelated
    {

        public static bool CheckValidSP(DiscoveredSavePoint sp)
        {

            if(Main.tile[sp.tileX, sp.tileY].type == ModContent.TileType<Tiles.savepoint>())
            {
                return true;
            }

            return false;

        }

    }

}
