﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace KingdomTerrahearts.Tiles
{
    class strugglePosterTile:ModTile
    {

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorLeft = AnchorData.Empty;
            TileObjectData.newTile.AnchorRight = AnchorData.Empty;
            TileObjectData.newTile.AnchorTop = AnchorData.Empty;
            TileObjectData.newTile.AnchorWall= true;
            TileObjectData.newTile.StyleHorizontal= true;
            TileObjectData.newTile.StyleWrapLimit = 42;
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Poster");
            AddMapEntry(new Color(100,100,0), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("strugglePosterItem"));
        }

    }
}
