using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace KingdomTerrahearts
{
    public class TileOverride : GlobalTile
    {

        public override bool Drop(int i, int j, int type)
        {
            EntitySource_TileBreak s = new EntitySource_TileBreak(i, j);
            switch (type)
            {
                case TileID.PalmTree:
                    if(Main.rand.Next(25) == 0)
                    {
                        Item.NewItem(s, i * 16, j * 16, 1, 1, ModContent.ItemType<Items.papouFruit>());
                    }
                    break;
                case TileID.Trees:
                    if(Main.rand.Next(15) == 0)
                    {
                        Item.NewItem(s, i * 16, j * 16, 1, 1, ModContent.ItemType<Items.Weapons.Joke.Keyblade_woodenStick>());
                    }
                    break;
                case TileID.Chlorophyte:
                    Item.NewItem(s, i * 16, j * 16, 1, 1, ItemID.ChlorophyteOre, Stack: Main.rand.Next(3, 15));
                    break;
            }

            return base.Drop(i, j, type);
        }

    }
}
