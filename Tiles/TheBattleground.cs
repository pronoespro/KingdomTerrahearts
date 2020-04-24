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
using static Terraria.ModLoader.ModContent;

namespace KingdomTerrahearts.Tiles
{
    public class TheBattleground :ModTile
    {

        public override void SetDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            Main.tileObsidianKill[Type] = false;

            Main.tileNoAttach[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.Width = 18;
            TileObjectData.newTile.Height = 28;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new int[28];
            for(int i = 0; i < TileObjectData.newTile.Height;i++)
            {
                TileObjectData.newTile.CoordinateHeights[i] = 16;
            }
            TileObjectData.newTile.Origin = new Point16(9, 27);
            TileObjectData.newTile.AnchorBottom= new AnchorData(AnchorType.SolidWithTop,4, TileObjectData.newTile.Width / 2 -2);
            TileObjectData.newTile.AnchorTop = AnchorData.Empty;
            TileObjectData.newTile.AnchorLeft = AnchorData.Empty;
            TileObjectData.newTile.AnchorRight = AnchorData.Empty;

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();

            name.SetDefault("The Battleground");
            AddMapEntry(new Color(255, 255, 255), name);

            animationFrameHeight = 504;
            dustType = 11;
            disableSmartCursor = true;
            minPick = 2;
            mineResist = 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i*16,j*16,9,28,mod.ItemType("TheBattlegroundItem"));
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            makeDust = false;
        }

    }
    public class TheBattlegroundItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Batlling Heart");
            Tooltip.SetDefault("A heart to call the Battlegrounds");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 13;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.rare = 1;
            item.createTile = mod.TileType("TheBattleground");
            item.placeStyle = 0;
            item.consumable = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("DarkenedHeart"));
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
