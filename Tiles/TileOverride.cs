using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace KingdomTerrahearts.Tiles
{
    public class TileOverride : GlobalTile
    {

        public override bool Drop(int i, int j, int type)
        {
            if (type == TileID.Trees && Main.rand.Next(15)==0)
            {
                Item.NewItem(i * 16, j * 16, 1, 1, mod.ItemType("Keyblade_woodenStick"));
            }
            if (type==TileID.Chlorophyte)
            {
                Item.NewItem(i * 16, j * 16, 1, 1, ItemID.ChlorophyteOre,Stack:Main.rand.Next(3,15));
            }
            return base.Drop(i, j, type);
        }

    }
}
