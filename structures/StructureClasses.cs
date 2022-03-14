using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace KingdomTerrahearts.structures
{

    #region classes
    public class StructureElement
    {
        public int[,] element;
        public int[] types;

        public StructureElement(int[,] elementValues,int[] typeArray)
        {
            element = elementValues;
            types = typeArray;
        }

        public static StructureElement emptyElement()
        {
            return new StructureElement(new int[0, 0], new int[0]);
        }
    }

    public class StructureComplete
    {
        public StructureElement walls;

        public StructureElement blocks;

        public int platformsType;

        public bool hasSlopes;
        public int[,] blockSlopes;

        public bool hasActuatedBlocks;
        public bool[,] actuatedBlocks;

        public bool hasBlockPaint;
        public StructureElement blockColors;
        public bool hasWallPaint;
        public StructureElement wallColors;

        public bool containsLiquids;
        public StructureElement liquids;

        public bool containsFurniture;
        public StructureElement furnitureTiles;
        public StructureElement furnitureColors;
        public StructureElement furnitureStyles;

        public bool containsChests;
        public StructureElement chests;
        public int[,] chestPosibleContent;
        public int structureChestStyle;

        public StructureComplete(StructureElement block,int[,] slopes, StructureElement blockCol, bool[,] actuated, StructureElement wall,StructureElement wallCol, StructureElement liquid, int platformtype=0)
        {

            blocks = block;

            hasSlopes = slopes.Length > 0;
            blockSlopes = slopes;

            hasBlockPaint = blockCol.element.Length > 0;
            blockColors = blockCol;

            hasActuatedBlocks = actuated.Length > 0;
            actuatedBlocks = actuated;

            hasWallPaint = wall.element.Length > 0;
            walls = wall;

            wallColors = wallCol;

            containsLiquids = liquid.element.Length>0;
            liquids = liquid;

            platformsType = platformtype;

            containsFurniture = false;

        }

        public StructureComplete(StructureElement block, int[,] slopes, StructureElement blockCol, bool[,] actuated, StructureElement wall, StructureElement wallCol, StructureElement liquid,StructureElement furniture, StructureElement funriturePaint,StructureElement furnitureStyle, int platformtype = 0)
        {

            blocks = block;

            hasSlopes = slopes.Length > 0;
            blockSlopes = slopes;

            hasBlockPaint = blockCol.element.Length > 0;
            blockColors = blockCol;

            hasActuatedBlocks = actuated.Length > 0;
            actuatedBlocks = actuated;

            hasWallPaint = wall.element.Length > 0;
            walls = wall;

            wallColors = wallCol;

            containsLiquids = liquid.element.Length > 0;
            liquids = liquid;

            platformsType = platformtype;

            containsFurniture = furniture.element.Length>0;
            furnitureTiles = furniture;
            furnitureColors = funriturePaint;
            furnitureStyles = furnitureStyle;

        }

        public StructureComplete(StructureElement block, int[,] slopes, StructureElement blockCol, bool[,] actuated, StructureElement wall, StructureElement wallCol, StructureElement liquid, StructureElement chest, int chestStyle, int[,] chestPossibleContents, int platformtype = 0)
        {

            blocks = block;

            hasSlopes = slopes.Length > 0;
            blockSlopes = slopes;

            hasBlockPaint = blockCol.element.Length > 0;
            blockColors = blockCol;

            hasActuatedBlocks = actuated.Length > 0;
            actuatedBlocks = actuated;

            hasWallPaint = wall.element.Length > 0;
            walls = wall;

            wallColors = wallCol;

            containsLiquids = liquid.element.Length > 0;
            liquids = liquid;

            platformsType = platformtype;

            containsChests = chest.element.Length > 0;
            chests = chest;
            chestPosibleContent = chestPossibleContents;
            structureChestStyle = chestStyle;

        }

        public StructureComplete(StructureElement block, int[,] slopes, StructureElement blockCol, bool[,] actuated, StructureElement wall, StructureElement wallCol, StructureElement liquid, StructureElement furniture, StructureElement funriturePaint, StructureElement furnitureStyle, StructureElement chest, int chestStyle, int[,] chestPossibleContents, int platformtype = 0)
        {

            blocks = block;

            hasSlopes = slopes.Length > 0;
            blockSlopes = slopes;

            hasBlockPaint = blockCol.element.Length > 0;
            blockColors = blockCol;

            hasActuatedBlocks = actuated.Length > 0;
            actuatedBlocks = actuated;

            hasWallPaint = wall.element.Length > 0;
            walls = wall;

            wallColors = wallCol;

            containsLiquids = liquid.element.Length > 0;
            liquids = liquid;

            platformsType = platformtype;

            containsFurniture = furniture.element.Length > 0;
            furnitureTiles = furniture;
            furnitureColors = funriturePaint;
            furnitureStyles = furnitureStyle;

            containsChests = chest.element.Length > 0;
            chests = chest;
            chestPosibleContent = chestPossibleContents;
            structureChestStyle = chestStyle;

        }

        public int ReturnLength(int dimension)
        {
            if (blocks.element.Rank < dimension)
            {
                return 0;
            }
            int range=0;
            switch (dimension)
            {
                default:
                case 0:
                    for (int i = 0; i < blocks.element.GetLength(0); i++)
                    {
                        range++;
                    }
                    break;
                case 1:
                    for (int i = 0; i < blocks.element.GetLength(1); i++)
                    {
                        range++;
                    }
                    break;
            }
            return range;
        }

        public static bool[,] BoolsFromInts(int[,] acInt)
        {
            bool[,] acBool = new bool[acInt.GetLength(0), acInt.GetLength(1)];

            for(int i = 0; i < acBool.GetLength(0); i++)
            {
                for(int j = 0; j < acBool.GetLength(1); j++)
                {
                    acBool[i, j] = acInt[i, j] > 0;
                }
            }

            return acBool;
        }

        public static bool[,] emptyActuated()
        {
            return new bool[0, 0];
        }
    }
    #endregion

    public static class StructureData
    {
        public static StructureComplete GetStructure(string name)
        {
            StructureElement blocks;
            StructureElement walls;
            StructureElement liquids = StructureElement.emptyElement();
            StructureElement blockPaint = StructureElement.emptyElement();
            StructureElement wallPaint = StructureElement.emptyElement();

            switch (name)
            {
                case "OlimpusCloud1":

                    blocks = new StructureElement(OlimpusCloud.OlimpusCloud1Blocks,OlimpusCloud.olimpusBlockTypes);
                    walls = new StructureElement( OlimpusCloud.OlimpusCloud1Walls, OlimpusCloud.olimpusWallTypes);

                    return new StructureComplete(blocks, OlimpusCloud.OlimpusCloud1Slopes,blockPaint, StructureComplete.emptyActuated(), walls,wallPaint, liquids);

                case "OlimpusCloud2":

                    blocks = new StructureElement(OlimpusCloud.OlimpusCloud2Blocks, OlimpusCloud.olimpusBlockTypes);
                    walls = new StructureElement(OlimpusCloud.OlimpusCloud2Walls, OlimpusCloud.olimpusWallTypes);
                    //wallPaint = new StructureElement(OlimpusCloud.OlimpusCoud2wallPaint, OlimpusCloud.olimpusPaintTypes);

                    return new StructureComplete(blocks, OlimpusCloud.OlimpusCloud2Slopes, blockPaint, StructureComplete.BoolsFromInts(OlimpusCloud.OlimpusCloud2Actuate), walls, wallPaint, liquids);

                case "OlimpusLake":

                    blocks = new StructureElement(OlimpusCloud.OlimpusLakeBlocks, OlimpusCloud.olimpusBlockTypes);
                    walls = new StructureElement(OlimpusCloud.OlimpusLakeWalls, OlimpusCloud.olimpusWallTypes);
                    liquids = new StructureElement(OlimpusCloud.OlimpusLakeLiquids,OlimpusCloud.olimpusliquidTypes);
                    blockPaint = new StructureElement(OlimpusCloud.OlimpusLakePaints,OlimpusCloud.olimpusPaintTypes);

                    return new StructureComplete(blocks, OlimpusCloud.OlimpusLakeSlopes, blockPaint, StructureComplete.BoolsFromInts(OlimpusCloud.OlimpusLakeActuate), walls,wallPaint, liquids,OlimpusCloud.olimpusPlatforms);

                default:
                    return null;
            }
        }
    }

}
